﻿using Elk.Core;
using Pharmacy.Domain;
using System;
using System.Threading.Tasks;

namespace Pharmacy.Service
{
    public interface IOrderService
    {
        Task<IResponse<(Order Order, bool IsChanged)>> AddByEndUserAsync(Guid userId, OrderDTO model);
        //Task<IResponse<Order>> AddTempBasket(TempOrderDTO model);
        //Task<bool> CheckOwner(Guid userId, int orderId);
        Task<IResponse<Order>> FindAsync(Guid OrderId);
        Task<IResponse<Order>> GetDetails(Guid OrderId);
        Task<IResponse<(string TrackingId, long OrderUniqueId)>> Verify(Payment payment, object[] args);
        Task<IResponse<Order>> Store_UpdateAsync(Store_OrderUpdateModel model);
        Task<IResponse<bool>> DeleteAsync(Guid OrderId);
        PagingListDetails<Order> Get(OrderSearchFilter filter);
        Task<IResponse<Order>> UpdateStatusAsync(Guid id, OrderStatus status, bool check = true);
        Response<GetDeliveryPriceDTO> GetDeliveryPrice(Guid id);
        Task<Response<Order>> CheckBeforeDeliveryPrice(Guid id);
    }
}