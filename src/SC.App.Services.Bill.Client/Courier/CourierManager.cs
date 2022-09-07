using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Bill.Common.Extensions;
using SC.App.Services.Courier.Client;

namespace SC.App.Services.Bill.Client.Courier
{
    public class CourierManager : ICourierManager
    {
        private readonly ICourierClient _courierClient;

        public CourierManager(
            ICourierClient courierClient)
        {
            _courierClient = courierClient;
        }

        public async Task<ResponseOfGetOrderResponse> GetOrderByIdAsync(HttpRequest request, Guid id)
        {
            _courierClient.SetAuthorization(request.GetAuthorization());
            _courierClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _courierClient.Orders_GetByIdAsync(id);
        }

        public async Task<ResponseOfGetOrderResponse> GetOrderByFilterAsync(HttpRequest request, Guid refId)
        {
            _courierClient.SetAuthorization(request.GetAuthorization());
            _courierClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _courierClient.Orders_GetByFilterAsync(refId, null);
        }

        public async Task<ResponseOfCreateOrderResponse> CreateOrderAsync(HttpRequest request, CreateOrder order)
        {
            _courierClient.SetAuthorization(request.GetAuthorization());
            _courierClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _courierClient.Orders_CreateOrderAsync(order);
        }

        public async Task<ResponseOfCancelOrderResponse> CancelOrderAsync(HttpRequest request, CancelOrder order)
        {
            _courierClient.SetAuthorization(request.GetAuthorization());
            _courierClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _courierClient.Orders_CancelOrderAsync(order);
        }
    }
}