using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Order.Client;

namespace SC.App.Services.Bill.Client.Order
{
    public interface IOrderManager
    {
        Task<ResponseOfListOfGetOrderResponse> GetOrderByLiveIdAsync(HttpRequest request, Guid liveId, EnumOrderStatus status);

        Task<ResponseOfListOfGetOrderResponse> GetOrderByBillIdAsync(HttpRequest request, Guid billId, EnumOrderStatus status);

        Task<ResponseOfCreateOrderResponse> CreateOrderAsync(HttpRequest request, Guid channelId, Guid? liveId, Guid? postId, Guid billId, CreateOrdersItem item, Guid userId);

        Task<ResponseOfUpdateOrdersResponse> UpdateOrderAsync(HttpRequest request, Guid channelId, Guid billId, List<UpdateOrdersItem> items, Guid userId);

        Task<ResponseOfPendingOrderResponse> PendingOrderAsync(HttpRequest request, Guid billId, Guid userId);

        Task<ResponseOfConfirmOrderResponse> ConfirmOrderAsync(HttpRequest request, Guid billId, Guid userId);

        Task<ResponseOfCancelOrderResponse> CancelOrderAsync(HttpRequest request, Guid billId, Guid userId);

        Task<ResponseOfUpdateParcelResponse> UpdateParcelAsync(HttpRequest request, Guid[] ids, Guid parcelId, Guid userId);

        Task<ResponseOfCancelParcelResponse> CancelParcelAsync(HttpRequest request, Guid parcelId, Guid userId);
    }
}