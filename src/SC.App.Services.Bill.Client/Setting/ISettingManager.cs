using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Setting.Client;

namespace SC.App.Services.Bill.Client.Setting
{
    public interface ISettingManager
    {
        Task<ResponseOfGetShopAccountResponse> GetShopAccountByChannelIdAsync(HttpRequest request, Guid channelId);

        Task<ResponseOfGetCourierAccountResponse> GetCourierAccountByChannelIdAsync(HttpRequest request, Guid channelId);

        Task<ResponseOfGetBillingResponse> GetBillingByChannelIdAsync(HttpRequest request, Guid channelId);

        Task<ResponseOfListOfGetCourierShippingResponse> GetCourierShippingsByCourierIdAsync(HttpRequest request, Guid courierId);

        Task<ResponseOfListOfGetCourierVelocityResponse> GetCourierVelocitiesByCourierIdAsync(HttpRequest request, Guid courierId);

        Task<ResponseOfListOfGetCourierPaymentResponse> GetCourierPaymentsByCourierIdAsync(HttpRequest request, Guid courierId);

        Task<ResponseOfListOfGetCourierInsuranceResponse> GetCourierInsurancesByCourierIdAsync(HttpRequest request, Guid courierId);

        Task<ResponseOfGetShippingResponse> GetShippingByChannelIdAsync(HttpRequest request, Guid channelId);

        Task<ResponseOfGetPaymentResponse> GetPaymentByChannelIdAsync(HttpRequest request, Guid channelId);

        Task<ResponseOfGetPreferencesResponse> GetPreferencesByChannelIdAsync(HttpRequest request, Guid channelId);
    }
}