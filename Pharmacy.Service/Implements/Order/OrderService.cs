﻿using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using Pharmacy.Service.Resource;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Pharmacy.Service
{
    public class OrderService : IOrderService
    {
        readonly AppUnitOfWork _appUow;
        readonly IGenericRepo<Order> _orderRepo;
        readonly IDrugService _drugSrv;
        readonly IDrugStoreService _drugStoreSrv;
        readonly IGatewayFactory _gatewayFactory;
        readonly IDeliveryAgentFactory _delAgent;
        readonly INotificationService _notifSrv;
        readonly IConfiguration _config;
        public OrderService(AppUnitOfWork appUOW,
            IGatewayFactory gatewayFactory,
            IDrugService drugSrv,
            IDrugStoreService drugStoreSrv,
            IDeliveryAgentFactory delAgent,
            INotificationService notifSrv,
            IConfiguration config)
        {
            _appUow = appUOW;
            _orderRepo = appUOW.OrderRepo;
            _drugSrv = drugSrv;
            _gatewayFactory = gatewayFactory;
            _drugStoreSrv = drugStoreSrv;
            _delAgent = delAgent;
            _notifSrv = notifSrv;
            _config = config;
        }

        public async Task<IResponse<(Order Order, bool IsChanged)>> AddByEndUserAsync(Guid userId, OrderDTO model)
        {
            var chkItems = _drugSrv.CheckChanges(model.Items);
            var orderItems = chkItems.Items.Select(i => new OrderItem
            {
                DrugId = i.DrugId,
                Count = i.Count,
                Price = i.Price,
                TotalPrice = i.GetTotalPrice(),
                DiscountPrice = i.Discount
            }).ToList();
            var order = new Order
            {
                TotalItemsPrice = orderItems.Sum(x => x.TotalPrice),
                TotalPriceWithoutDiscount = orderItems.Sum(x => x.Price * x.Count),
                TotalPrice = orderItems.Sum(x => x.TotalPrice),
                TotalDiscountPrice = orderItems.Sum(x => x.DiscountPrice),
                UserId = userId,
                Status = OrderStatus.WaitForPayment,
                DeliveryType = model.DeliveryType,
                Comment = model.Comment,
                ExtraInfoJson = "",
                AddressId = model.Address.Id ?? 0,
                Address = model.Address.Id == null ? new UserAddress
                {
                    UserId = userId,
                    Latitude = model.Address.Latitude,
                    Longitude = model.Address.Longitude,
                    Details = model.Address.Details
                } : null,
                OrderItems = orderItems
            };
            if (chkItems.Changed) return new Response<(Order Order, bool IsChanged)>
            {
                Result = (order, chkItems.Changed),
                Message = ServiceMessage.OrderChanged
            };
            var nearest = _drugStoreSrv.GetNearest(model.Address);
            if (!nearest.IsSuccessful) return new Response<(Order Order, bool IsChanged)>
            {
                Message = nearest.Message,
                Result = (null, false)
            };
            order.Store_UserId = nearest.Result.UserId;
            var delAgent = _delAgent.Get(model.DeliveryType);
            var getPrice = await delAgent.PriceInquiry(nearest.Result, model.Address, false, false);
            if (!getPrice.IsSuccessful)
                return new Response<(Order Order, bool IsChanged)>
                {
                    Message = getPrice.Message,
                    Result = (null, false)
                };
            order.OrderDrugStores = new List<OrderDrugStore> {
                    new OrderDrugStore{
                        DrugStoreId = nearest.Result.DrugStoreId,
                        Status = OrderDrugStoreStatus.InProccessing,
                        DeliveryPrice = getPrice.Result.Price
                    }
                };
            order.DeliveryAgentName = delAgent.Name;
            order.DrugStoreId = nearest.Result.DrugStoreId;
            await _orderRepo.AddAsync(order);
            var addOrder = await _appUow.ElkSaveChangesAsync();
            if (!addOrder.IsSuccessful)
                return new Response<(Order, bool)>
                {
                    Message = addOrder.Message,
                    Result = (null, false)
                };
            return new Response<(Order, bool)>
            {
                IsSuccessful = true,
                Result = (order, chkItems.Changed)
            };
        }

        //public async Task<IResponse<Order>> AddTempBasket(TempOrderDTO model)
        //{
        //    var getItems = _TempBasketItemSrv.Get(model.BasketId);
        //    if (!getItems.IsSuccessful) return new Response<Order> { Message = getItems.Message };
        //    var DrugId = getItems.Result.Where(x => x.Count != 0).First().DrugId;
        //    var drugStore = _drugStoreSrv.GetNearest(model.Address);
        //    if (!drugStore.IsSuccessful) return new Response<Order> { Message = drugStore.Message };
        //    var getDeliveryCost = await _deliverySrv.GetDeliveryCost(model.DeliveryId, drugStore.Result.DrugStoreId, new LocationDTO { Lat = model.Address.Lat, Lng = model.Address.Lng });
        //    if (!getDeliveryCost.IsSuccessful) return new Response<Order> { Message = getDeliveryCost.Message };
        //    var orderItems = getItems.Result.Where(x => x.Count != 0).Select(i => new OrderItem
        //    {
        //        DrugId = i.DrugId,
        //        Count = i.Count,
        //        Price = i.Price,
        //        TotalPrice = i.GetTotalPrice(),
        //        DiscountPrice = i.DiscountPrice
        //    }).ToList();
        //    var order = new Order
        //    {
        //        TotalItemsPrice = orderItems.Sum(x => x.TotalPrice),
        //        TotalDiscountPrice = orderItems.Sum(x=>x.DiscountPrice),
        //        TotalPriceWithoutDiscount = orderItems.Sum(x => x.Price * x.Count) + getDeliveryCost.Result,
        //        TotalPrice = orderItems.Sum(x => x.TotalPrice) + getDeliveryCost.Result,
        //        UserId = model.UserToken,
        //        OrderStatus = OrderStatus.WaitForPayment,
        //        DeliveryProviderId = model.DeliveryId,
        //        Description = model.Description,
        //        ExtraInfoJson = "",//new ExtraInfo { Reciever = model.Reciever, RecieverMobileNumber = model.RecieverMobileNumber }.SerializeToJson(),
        //        AddressId = model.Address.Id ?? 0,
        //        Address = model.Address.Id == null ? new UserAddress
        //        {
        //            UserId = model.UserToken,
        //            Latitude = model.Address.Lat,
        //            Longitude = model.Address.Lng,
        //            Details = model.Address.Details
        //        } : null,
        //        OrderDetails = orderItems
        //    };
        //    await _orderRepo.AddAsync(order);
        //    var addOrder = await _appUow.ElkSaveChangesAsync();
        //    if (!addOrder.IsSuccessful)
        //        return new Response<Order> { Message = addOrder.Message };
        //    return new Response<Order>
        //    {
        //        IsSuccessful = true,
        //        Result = order
        //    };
        //}

        // public async Task<bool> CheckOwner(Guid userId, int orderId) => await _orderRepo.AnyAsync(x => x.OrderId == orderId && x.OrderDrugStores.Any(o=>o.User == UserId == userId);

        public async Task<IResponse<Order>> FindAsync(Guid OrderId)
        {
            var order = await _appUow.OrderRepo.FirstOrDefaultAsync(new BaseFilterModel<Order>
            {
                Conditions = x => x.OrderId == OrderId,
                IncludeProperties = new List<Expression<Func<Order, object>>>
                {
                    x=>x.Address,
                    x=>x.Address.User
                }
            });
            if (order == null) return new Response<Order> { Message = ServiceMessage.RecordNotExist };
            return new Response<Order> { Result = order, IsSuccessful = true };
        }

        public async Task<IResponse<string>> Verify(Payment payment, object[] args)
        {
            var findGateway = await _gatewayFactory.GetInsance(payment.PaymentGatewayId);
            if (!findGateway.IsSuccessful)
                return new Response<string> { Message = ServiceMessage.RecordNotExist };
            var verify = await findGateway.Result.Service.VerifyTransaction(new VerifyRequest
            {
                OrderId = payment.PaymentId,
                TransactionId = payment.TransactionId,
                ApiKey = findGateway.Result.Gateway.MerchantId,
                Url = findGateway.Result.Gateway.VerifyUrl
            }, args);
            if (!verify.IsSuccessful) return new Response<string> { IsSuccessful = false, Result = payment.TransactionId };
            var order = await _orderRepo.FirstOrDefaultAsync(new BaseFilterModel<Order>
            {
                Conditions = x => x.OrderId == payment.OrderId,
                IncludeProperties = new List<Expression<Func<Order, object>>> { x => x.Address, x => x.Address.User }
            });
            if (order == null) return new Response<string> { Message = ServiceMessage.RecordNotExist };
            order.Status = OrderStatus.InProcessing;
            payment.PaymentStatus = PaymentStatus.Success;
            _appUow.OrderRepo.Update(order);
            _appUow.PaymentRepo.Update(payment);
            var update = await _appUow.ElkSaveChangesAsync();
            if (update.IsSuccessful)
                await HandleStatusChange(order);
            return new Response<string>
            {
                IsSuccessful = update.IsSuccessful,
                Result = payment.TransactionId,
                Message = update.Message

            };
        }

        public async Task<IResponse<Order>> AddAsync(Order model)
        {
            await _orderRepo.AddAsync(model);

            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<Order> { Result = model, IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };
        }

        public async Task<IResponse<Order>> Store_UpdateAsync(Store_OrderUpdateModel model)
        {
            var order = await _orderRepo.FirstOrDefaultAsync(new BaseFilterModel<Order>
            {
                Conditions = x => x.OrderId == model.OrderId && !x.IsDeleted,
                IncludeProperties = new List<Expression<Func<Order, object>>> { x => x.Address, x => x.Address.User, x => x.OrderDrugStores }
            });
            if (order == null) return new Response<Order> { Message = ServiceMessage.RecordNotExist };
            var orderDrugStore = order.OrderDrugStores.OrderByDescending(x => x.OrderDrugStoreId).FirstOrDefault();
            orderDrugStore.Comment = model.OrderDrugStoreComment;
            var sendNotif = false;
            if (model.OrderDrugStoreStatus == OrderDrugStoreStatus.Accepted)
            {
                orderDrugStore.Status = OrderDrugStoreStatus.Accepted;
                order.Status = OrderStatus.Accepted;
                sendNotif = true;
            }
            else
            {
                orderDrugStore.Status = OrderDrugStoreStatus.Denied;
                if (order.OrderDrugStores.Count < 3)
                {
                    var nearest = _drugStoreSrv.GetNearest(order.Address, order.OrderDrugStores.Select(x => x.DrugStoreId).ToList());
                    if (!nearest.IsSuccessful) return new Response<Order> { Message = nearest.Message };
                    var delAgent = _delAgent.Get(order.DeliveryType);
                    var getPrice = await delAgent.PriceInquiry(nearest.Result, order.Address, false, false);
                    if (!getPrice.IsSuccessful) return new Response<Order> { Message = getPrice.Message };
                    order.Store_UserId = nearest.Result.UserId;
                    order.OrderDrugStores.Add(new OrderDrugStore
                    {
                        DrugStoreId = nearest.Result.DrugStoreId,
                        Status = OrderDrugStoreStatus.InProccessing,
                        DeliveryPrice = getPrice.Result.Price
                    });
                }
            }
            _orderRepo.Update(order);
            //TODO:Set Update Fileds
            var update = await _appUow.ElkSaveChangesAsync();
            if (update.IsSuccessful && sendNotif) await HandleStatusChange(order);
            return new Response<Order> { Result = order, IsSuccessful = update.IsSuccessful, Message = update.Message };
        }

        public async Task<IResponse<Order>> UpdateStatusAsync(Guid id, OrderStatus status, bool check = true)
        {
            var order = await _orderRepo.FindAsync(id);
            if (order == null) return new Response<Order> { Message = ServiceMessage.RecordNotExist };
            if (check && (order.Status == OrderStatus.Done || (order.Status == OrderStatus.WaitForPayment && order.Status != status)))
                return new Response<Order> { Message = ServiceMessage.NotAllowedOperation };
            order.Status = status;
            _orderRepo.Update(order);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<Order> { IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };

        }

        public async Task<IResponse<bool>> DeleteAsync(Guid OrderId)
        {
            _appUow.OrderRepo.Delete(new Order { OrderId = OrderId });
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<bool>
            {
                Message = saveResult.Message,
                Result = saveResult.IsSuccessful,
                IsSuccessful = saveResult.IsSuccessful,
            };
        }

        public PagingListDetails<Order> Get(OrderSearchFilter filter)
        {
            Expression<Func<Order, bool>> conditions = x => true;
            if (filter != null)
            {
                if (filter.UserId != null)
                    conditions = conditions.And(x => x.Store_UserId == filter.UserId &&
                    (x.Status == OrderStatus.Accepted || x.Status == OrderStatus.InProcessing) &&
                    x.OrderDrugStores.OrderByDescending(x=>x.OrderDrugStoreId).First().Status != OrderDrugStoreStatus.Denied);
                if (filter.DrugStoreId != null) conditions = conditions.And(x => x.OrderDrugStores.Any(ods => ods.DrugStoreId == filter.DrugStoreId && (ods.Status == OrderDrugStoreStatus.Accepted || ods.Status == OrderDrugStoreStatus.InProccessing)));
                if (filter.UniqueId != null) conditions = conditions.And(x => x.UniqueId == filter.UniqueId);
                if (!string.IsNullOrWhiteSpace(filter.FromDateSh))
                {
                    var dt = PersianDateTime.Parse(filter.FromDateSh).ToDateTime();
                    conditions = conditions.And(x => x.InsertDateMi >= dt);
                }
                if (!string.IsNullOrWhiteSpace(filter.ToDateSh))
                {
                    var dt = PersianDateTime.Parse(filter.ToDateSh).ToDateTime();
                    conditions = conditions.And(x => x.InsertDateMi <= dt);
                }
                if (!string.IsNullOrWhiteSpace(filter.TransactionId))
                    conditions = conditions.And(x => x.Payments.Any(p => p.TransactionId == filter.TransactionId));
                if (filter.Status != null)
                    conditions = conditions.And(x => x.Status == filter.Status);
                if (filter.OrderDrugStoreStatus != null)
                    conditions = conditions.And(x => x.OrderDrugStores.OrderByDescending(x => x.OrderDrugStoreId).First().Status == filter.OrderDrugStoreStatus);
            }

            return _orderRepo.Get(new BasePagedListFilterModel<Order>
            {
                Conditions = conditions,
                PagingParameter = filter,
                OrderBy = x => x.OrderByDescending(i => i.OrderId),
                IncludeProperties = new List<Expression<Func<Order, object>>>
                {
                    x=>x.OrderDrugStores,
                    x=>x.Address,
                    x=>x.Address.User,
                    x=>x.OrderDrugStores
                }
            });
        }

        public async Task<IResponse<Order>> GetDetails(Guid OrderId)
        {
            var result = await _appUow.OrderRepo.FirstOrDefaultAsync(new FilterModel<Order, dynamic>
            {
                Selector = x => new
                {
                    x.OrderId,
                    x.TotalPrice,
                    x.TotalDiscountPrice,
                    x.Address,
                    x.Address.User,
                    x.Status,
                    x.InsertDateSh,
                    x.Comment,
                    x.UniqueId,
                    DrugStore = x.OrderDrugStores.OrderByDescending(d => d.OrderDrugStoreId).Select(d => d.DrugStore).FirstOrDefault()
                },
                Conditions = x => x.OrderId == OrderId,
                IncludeProperties = new List<Expression<Func<Order, object>>>
                {
                    x=>x.Address,
                    x=>x.Address.User
                }
            });
            if (result == null) return new Response<Order> { Message = ServiceMessage.RecordNotExist };
            return new Response<Order>
            {
                Result = new Order
                {
                    OrderId = result.OrderId,
                    Status = result.Status,
                    InsertDateSh = result.InsertDateSh,
                    Comment = result.Comment,
                    TotalPrice = result.TotalPrice,
                    TotalDiscountPrice = result.TotalDiscountPrice,
                    Address = result.Address,
                    DrugStore = result.DrugStore,
                    UniqueId = result.UniqueId,
                    OrderItems = _appUow.OrderDetailRepo.Get(new BaseListFilterModel<OrderItem>
                    {
                        Conditions = x => x.OrderId == OrderId,
                        OrderBy = o => o.OrderByDescending(x => x.OrderItemId),
                        IncludeProperties = new List<Expression<Func<OrderItem, object>>>
                        {
                            x => x.Drug
                        }
                    }),
                    Payments = _appUow.PaymentRepo.Get(new BaseListFilterModel<Payment>
                    {
                        Conditions = x => x.OrderId == OrderId,
                        OrderBy = o => o.OrderByDescending(x => x.PaymentId),
                        IncludeProperties = new List<Expression<Func<Payment, object>>>
                            {
                                x=>x.PaymentGateway
                            }
                    })
                },
                IsSuccessful = true
            };
        }

        public async Task<IResponse<bool>> HandleStatusChange(Order order)
        {
            switch (order.Status)
            {
                case OrderStatus.Accepted:
                    return await _notifSrv.NotifyAsync(new NotificationDto
                    {
                        Email = order.Address.User.Email,
                        Content = string.Format(NotifierMessage.DrugStoreAcceptOrder,
                        ServiceMessage.ProjectName,
                        $"{_config["CustomSettings:ReactDeliveryPaymentUrl"]}/{order.OrderId}"),
                        FullName = order.Address.User.FullName,
                        MobileNumber = order.Address.User.MobileNumber,
                        Type = EventType.Order
                    });
                case OrderStatus.InProcessing:
                    return await _notifSrv.NotifyAsync(new NotificationDto
                    {
                        Email = order.Address.User.Email,
                        Content = string.Format(NotifierMessage.OrderPayment, order.UniqueId),
                        FullName = order.Address.User.FullName,
                        MobileNumber = order.Address.User.MobileNumber,
                        Type = EventType.Order
                    });
                case OrderStatus.Canceled:
                    return await _notifSrv.NotifyAsync(new NotificationDto
                    {
                        Email = order.Address.User.Email,
                        Content = string.Format(NotifierMessage.OrderCanceled, order.UniqueId),
                        FullName = order.Address.User.FullName,
                        MobileNumber = order.Address.User.MobileNumber,
                        Type = EventType.Order
                    });
                default:
                    return new Response<bool> { IsSuccessful = true };
            }

        }
    }
}
