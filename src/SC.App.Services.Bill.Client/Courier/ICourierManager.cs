using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Courier.Client;

namespace SC.App.Services.Bill.Client.Courier
{
    public interface ICourierManager
    {
        Task<ResponseOfGetOrderResponse> GetOrderByIdAsync(HttpRequest request, Guid id);

        Task<ResponseOfGetOrderResponse> GetOrderByFilterAsync(HttpRequest request, Guid refId);

        Task<ResponseOfCreateOrderResponse> CreateOrderAsync(HttpRequest request, CreateOrder order);

        Task<ResponseOfCancelOrderResponse> CancelOrderAsync(HttpRequest request, CancelOrder order);
    }
}