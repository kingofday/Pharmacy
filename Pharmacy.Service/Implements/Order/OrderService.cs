using Elk.Core;
using System.Linq;
using Pharmacy.Domain;
using Pharmacy.DataAccess.Ef;
using System.Threading.Tasks;
using Pharmacy.Service.Resource;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;

namespace Pharmacy.Service
{
    public class OrderService : IOrderService
    {
        readonly AppUnitOfWork _appUow;
        readonly IGenericRepo<Order> _orderRepo;
        readonly IDrugService _drugSrv;
        readonly IDrugStoreService _drugStoreSrv;
        readonly IGatewayFactory _gatewayFactory;
        readonly IDeliveryService _deliverySrv;
        readonly ITempBasketItemService _TempBasketItemSrv;
        public OrderService(AppUnitOfWork appUOW,
            IGatewayFactory gatewayFactory,
            IDrugService drugSrv,
            IDrugStoreService drugStoreSrv,
            IDeliveryService deliverySrv,
            ITempBasketItemService TempBasketItemSrv)
        {
            _appUow = appUOW;
            _orderRepo = appUOW.OrderRepo;
            _drugSrv = drugSrv;
            _gatewayFactory = gatewayFactory;
            _deliverySrv = deliverySrv;
            _TempBasketItemSrv = TempBasketItemSrv;
            _drugStoreSrv = drugStoreSrv;
        }

        public async Task<IResponse<(Order Order, bool IsChanged)>> AddByUserAsync(OrderDTO model)
        {
            var chkItems = await _drugSrv.CheckChanges(model.Items);
            var drugStore = _drugStoreSrv.GetNearest(model.Address);
            if (!drugStore.IsSuccessful)
                return new Response<(Order Order, bool IsChanged)>
                {
                    Message = drugStore.Message
                };

            var getDeliveryCost = await _deliverySrv.GetDeliveryCost(model.DeliveryId, drugStore.Result.DrugStoreId, new LocationDTO { Lat = model.Address.Lat, Lng = model.Address.Lng });
            if (!getDeliveryCost.IsSuccessful) return new Response<(Order, bool)> { Message = getDeliveryCost.Message };
            var orderItems = chkItems.Items.Where(x => x.Count != 0).Select(i => new OrderItem
            {
                DrugId = i.Id,
                Count = i.Count,
                Price = i.Price,
                TotalPrice = i.GetTotalPrice(),
                DiscountPrice = i.DiscountPrice
            }).ToList();
            var order = new Order
            {
                TotalItemsPrice = orderItems.Sum(x => x.TotalPrice),
                TotalPriceWithoutDiscount = orderItems.Sum(x => x.Price * x.Count) + getDeliveryCost.Result,
                TotalPrice = orderItems.Sum(x => x.TotalPrice) + getDeliveryCost.Result,
                TotalDiscountPrice = orderItems.Sum(x => x.DiscountPrice),
                UserId = model.UserToken,
                OrderStatus = OrderStatus.WaitForPayment,
                DeliveryProviderId = model.DeliveryId,
                Description = model.Description,
                ExtraInfoJson = new ExtraInfo { Reciever = model.Reciever, RecieverMobileNumber = model.RecieverMobileNumber }.SerializeToJson(),
                AddressId = model.Address.Id ?? 0,
                DrugStoreId = drugStore.Result.DrugStoreId,
                OrderDrugStores = new List<OrderDrugStore> {
                    new OrderDrugStore{
                        DrugStoreId = drugStore.Result.DrugStoreId,
                        Status = OrderPharmacyStatus.Proccessing,
                        DeliveryPrice = getDeliveryCost.Result
                    }
                },
                Address = model.Address.Id == null ? new UserAddress
                {
                    UserId = model.UserToken,
                    Latitude = model.Address.Lat,
                    Longitude = model.Address.Lng,
                    AddressDetails = model.Address.Address
                } : null,
                OrderDetails = orderItems
            };
            await _orderRepo.AddAsync(order);
            var addOrder = await _appUow.ElkSaveChangesAsync();
            if (!addOrder.IsSuccessful)
                return new Response<(Order, bool)> { Message = addOrder.Message };
            return new Response<(Order, bool)>
            {
                IsSuccessful = true,
                Result = (order, chkItems.Changed)
            };
        }

        public async Task<IResponse<Order>> AddTempBasket(TempOrderDTO model)
        {
            var getItems = _TempBasketItemSrv.Get(model.BasketId);
            if (!getItems.IsSuccessful) return new Response<Order> { Message = getItems.Message };
            var DrugId = getItems.Result.Where(x => x.Count != 0).First().DrugId;
            var drugStore = _drugStoreSrv.GetNearest(model.Address);
            if (!drugStore.IsSuccessful) return new Response<Order> { Message = drugStore.Message };
            var getDeliveryCost = await _deliverySrv.GetDeliveryCost(model.DeliveryId, drugStore.Result.DrugStoreId, new LocationDTO { Lat = model.Address.Lat, Lng = model.Address.Lng });
            if (!getDeliveryCost.IsSuccessful) return new Response<Order> { Message = getDeliveryCost.Message };
            var orderItems = getItems.Result.Where(x => x.Count != 0).Select(i => new OrderItem
            {
                DrugId = i.DrugId,
                Count = i.Count,
                Price = i.Price,
                TotalPrice = i.GetTotalPrice(),
                DiscountPrice = i.DiscountPrice
            }).ToList();
            var order = new Order
            {
                TotalItemsPrice = orderItems.Sum(x => x.TotalPrice),
                TotalDiscountPrice = orderItems.Sum(x=>x.DiscountPrice),
                TotalPriceWithoutDiscount = orderItems.Sum(x => x.Price * x.Count) + getDeliveryCost.Result,
                TotalPrice = orderItems.Sum(x => x.TotalPrice) + getDeliveryCost.Result,
                UserId = model.UserToken,
                OrderStatus = OrderStatus.WaitForPayment,
                DeliveryProviderId = model.DeliveryId,
                Description = model.Description,
                ExtraInfoJson = new ExtraInfo { Reciever = model.Reciever, RecieverMobileNumber = model.RecieverMobileNumber }.SerializeToJson(),
                AddressId = model.Address.Id ?? 0,
                Address = model.Address.Id == null ? new UserAddress
                {
                    UserId = model.UserToken,
                    Latitude = model.Address.Lat,
                    Longitude = model.Address.Lng,
                    AddressDetails = model.Address.Address
                } : null,
                OrderDetails = orderItems
            };
            await _orderRepo.AddAsync(order);
            var addOrder = await _appUow.ElkSaveChangesAsync();
            if (!addOrder.IsSuccessful)
                return new Response<Order> { Message = addOrder.Message };
            return new Response<Order>
            {
                IsSuccessful = true,
                Result = order
            };
        }

        //public async Task<bool> CheckOwner(Guid userId, int orderId) => await _orderRepo.AnyAsync(x => x.OrderId == orderId && x.Pharmacy.UserId == userId);

        //public async Task<IResponse<Order>> FindAsync(int OrderId)
        //{
        //    var order = await _appUow.OrderRepo.FirstOrDefaultAsync(x => x.OrderId == OrderId, new System.Collections.Generic.List<Expression<Func<Order, object>>>
        //    {
        //        x=>x.Pharmacy,
        //        x=>x.User,
        //        x=>x.FromAddress
        //    });
        //    if (order == null) return new Response<Order> { Message = ServiceMessage.RecordNotExist };
        //    return new Response<Order> { Result = order, IsSuccessful = true };
        //}

        //public async Task<IResponse<Order>> GetDetails(int OrderId)
        //{
        //    var order = await _appUow.OrderRepo.FirstOrDefaultAsync(x => x.OrderId == OrderId, new System.Collections.Generic.List<Expression<Func<Order, object>>>
        //    {
        //        x=>x.Pharmacy,
        //        x=>x.User,
        //        x=>x.ToAddress
        //    });
        //    if (order == null) return new Response<Order> { Message = ServiceMessage.RecordNotExist };
        //    order.OrderDetails = _appUow.OrderDetailRepo.Get(x => x.OrderId == OrderId, o => o.OrderByDescending(x => x.OrderDetailId), new System.Collections.Generic.List<Expression<Func<OrderDetail, object>>>
        //    {
        //        x=>x.Drug
        //    });
        //    order.Payments = _appUow.PaymentRepo.Get(x => x.OrderId == OrderId, o => o.OrderByDescending(x => x.PaymentId), new System.Collections.Generic.List<Expression<Func<Payment, object>>>
        //    {
        //        x=>x.PaymentGateway
        //    });
        //    return new Response<Order> { Result = order, IsSuccessful = true };
        //}

        public async Task<IResponse<string>> Verify(Payment payment, object[] args)
        {
            var findGateway = await _gatewayFactory.GetInsance(payment.PaymentGatewayId);
            if (!findGateway.IsSuccessful)
                return new Response<string> { Message = ServiceMessage.RecordNotExist };
            var verify = await findGateway.Result.Service.VerifyTransaction(new VerifyRequest
            {
                OrderId = payment.OrderId,
                TransactionId = payment.TransactionId,
                ApiKey = findGateway.Result.Gateway.MerchantId,
                Url = findGateway.Result.Gateway.VerifyUrl
            }, args);
            if (!verify.IsSuccessful) return new Response<string> { IsSuccessful = false, Result = payment.TransactionId };
            var order = await _orderRepo.FindAsync(payment.OrderId);
            if (order == null) return new Response<string> { Message = ServiceMessage.RecordNotExist };
            order.OrderStatus = OrderStatus.InProcessing;
            payment.PaymentStatus = PaymentStatus.Success;
            _appUow.OrderRepo.Update(order);
            _appUow.PaymentRepo.Update(payment);
            var update = await _appUow.ElkSaveChangesAsync();

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

        public async Task<IResponse<Order>> UpdateAsync(Order model)
        {
            var findedOrder = await _orderRepo.FindAsync(model.OrderId);
            if (findedOrder == null) return new Response<Order> { Message = ServiceMessage.RecordNotExist };
            //TODO:Set Update Fileds
            var saveResult = _appUow.ElkSaveChangesAsync();
            return new Response<Order> { Result = findedOrder, IsSuccessful = saveResult.Result.IsSuccessful, Message = saveResult.Result.Message };
        }

        public async Task<IResponse<Order>> UpdateStatusAsync(int id, OrderStatus status, bool check = true)
        {
            var order = await _orderRepo.FindAsync(id);
            if (order == null) return new Response<Order> { Message = ServiceMessage.RecordNotExist };
            if (check && (order.OrderStatus == OrderStatus.Successed || (order.OrderStatus == OrderStatus.WaitForPayment && order.OrderStatus != status)))
                return new Response<Order> { Message = ServiceMessage.NotAllowedOperation };
            order.OrderStatus = status;
            _orderRepo.Update(order);
            var saveResult = await _appUow.ElkSaveChangesAsync();
            return new Response<Order> { IsSuccessful = saveResult.IsSuccessful, Message = saveResult.Message };

        }

        public async Task<IResponse<bool>> DeleteAsync(int OrderId)
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
                if (filter.UserId != null) conditions = conditions.And(x => x.UserId == filter.UserId);
                if (filter.DrugStoreId != null) conditions = conditions.And(x => x.OrderDrugStores.Any(ods => ods.DrugStoreId == filter.DrugStoreId && ods.Status == OrderPharmacyStatus.Accepted));
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
                if (filter.OrderStatus != null)
                    conditions = conditions.And(x => x.OrderStatus == filter.OrderStatus);
            }

            return _orderRepo.Get(conditions, filter, x => x.OrderByDescending(i => i.OrderId), new System.Collections.Generic.List<Expression<Func<Order, object>>>
            {
                x=>x.OrderDrugStores,
                x=>x.Address,
                x=>x.User
            });
        }
    }
}
