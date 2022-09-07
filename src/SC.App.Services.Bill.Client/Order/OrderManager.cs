using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Bill.Common.Extensions;
using SC.App.Services.Order.Client;

namespace SC.App.Services.Bill.Client.Order
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderClient _orderClient;

        public OrderManager(
            IOrderClient orderClient)
        {
            _orderClient = orderClient;
        }

        public async Task<ResponseOfListOfGetOrderResponse> GetOrderByLiveIdAsync(HttpRequest request, Guid liveId, EnumOrderStatus status)
        {
            _orderClient.SetAuthorization(request.GetAuthorization());
            _orderClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _orderClient.Orders_GetByFilterAsync(liveId, null, null, null, null, new Guid[] { }, status);
        }

        public async Task<ResponseOfListOfGetOrderResponse> GetOrderByBillIdAsync(HttpRequest request, Guid billId, EnumOrderStatus status)
        {
            _orderClient.SetAuthorization(request.GetAuthorization());
            _orderClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _orderClient.Orders_GetByFilterAsync(null, null, billId, null, null, new Guid[] { }, status);
        }

        public async Task<ResponseOfCreateOrderResponse> CreateOrderAsync(HttpRequest request, Guid channelId, Guid? liveId, Guid? postId, Guid billId, CreateOrdersItem item, Guid userId)
        {
            _orderClient.SetAuthorization(request.GetAuthorization());
            _orderClient.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new CreateOrder
            {
                Channel_id = channelId,
                Live_id = liveId,
                Post_id = postId,
                Bill_id = billId,
                Booking_id = item.Booking_id,
                Product_id = item.Product_id,
                Code = item.Code,
                Amount = item.Amount,
                Unit_price = item.Unit_price,
                User_id = userId
            };

            return await _orderClient.Orders_CreateAsync(payload);
        }

        public async Task<ResponseOfUpdateOrdersResponse> UpdateOrderAsync(HttpRequest request, Guid channelId, Guid billId, List<UpdateOrdersItem> items, Guid userId)
        {
            _orderClient.SetAuthorization(request.GetAuthorization());
            _orderClient.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new UpdateOrders
            {
                Channel_id = channelId,
                Live_id = null,
                Post_id = null,
                Bill_id = billId,
                Items = items,
                User_id = userId
            };
            
            return await _orderClient.Orders_BulkUpdateAsync(payload);
        }

        public async Task<ResponseOfPendingOrderResponse> PendingOrderAsync(HttpRequest request, Guid billId, Guid userId)
        {
            _orderClient.SetAuthorization(request.GetAuthorization());
            _orderClient.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new PendingOrderByBillId
            {
                Bill_id = billId,
                User_id = userId
            };

            return await _orderClient.Orders_PendingByBillIdAsync(payload);
        }

        public async Task<ResponseOfConfirmOrderResponse> ConfirmOrderAsync(HttpRequest request, Guid billId, Guid userId)
        {
            _orderClient.SetAuthorization(request.GetAuthorization());
            _orderClient.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new ConfirmOrderByBillId
            {
                Bill_id = billId,
                User_id = userId
            };

            return await _orderClient.Orders_ConfirmByBillIdAsync(payload);
        }

        public async Task<ResponseOfCancelOrderResponse> CancelOrderAsync(HttpRequest request, Guid billId, Guid userId)
        {
            _orderClient.SetAuthorization(request.GetAuthorization());
            _orderClient.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new CancelOrderByBillId
            {
                Bill_id = billId,
                User_id = userId
            };

            return await _orderClient.Orders_CancelByBillIdAsync(payload);
        }

        public async Task<ResponseOfUpdateParcelResponse> UpdateParcelAsync(HttpRequest request, Guid[] ids, Guid parcelId, Guid userId)
        {
            _orderClient.SetAuthorization(request.GetAuthorization());
            _orderClient.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new UpdateParcel
            {
                Ids = ids,
                Parcel_id = parcelId,
                User_id = userId
            };

            return await _orderClient.Orders_UpdateParcelAsync(payload);
        }

        public async Task<ResponseOfCancelParcelResponse> CancelParcelAsync(HttpRequest request, Guid parcelId, Guid userId)
        {
            _orderClient.SetAuthorization(request.GetAuthorization());
            _orderClient.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new CancelParcel
            {
                Parcel_id = parcelId,
                User_id = userId
            };

            return await _orderClient.Orders_CancleParcelAsync(parcelId, payload);
        }
    }
}