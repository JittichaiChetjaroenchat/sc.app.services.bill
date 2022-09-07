using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Bill.Common.Extensions;
using SC.App.Services.Setting.Client;

namespace SC.App.Services.Bill.Client.Setting
{
    public class SettingManager : ISettingManager
    {
        private readonly ISettingClient _settingClient;

        public SettingManager(
            ISettingClient settingClient)
        {
            _settingClient = settingClient;
        }

        public async Task<ResponseOfGetShopAccountResponse> GetShopAccountByChannelIdAsync(HttpRequest request, Guid channelId)
        {
            _settingClient.SetAuthorization(request.GetAuthorization());
            _settingClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _settingClient.ShopAccounts_GetByFilterAsync(channelId);
        }

        public async Task<ResponseOfGetCourierAccountResponse> GetCourierAccountByChannelIdAsync(HttpRequest request, Guid channelId)
        {
            _settingClient.SetAuthorization(request.GetAuthorization());
            _settingClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _settingClient.CourierAccounts_GetByFilterAsync(channelId);
        }

        public async Task<ResponseOfGetBillingResponse> GetBillingByChannelIdAsync(HttpRequest request, Guid channelId)
        {
            _settingClient.SetAuthorization(request.GetAuthorization());
            _settingClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _settingClient.Billings_GetByFilterAsync(channelId);
        }

        public async Task<ResponseOfListOfGetCourierShippingResponse> GetCourierShippingsByCourierIdAsync(HttpRequest request, Guid courierId)
        {
            _settingClient.SetAuthorization(request.GetAuthorization());
            _settingClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _settingClient.Couriers_GetShippingByCourierIdAsync(courierId);
        }

        public async Task<ResponseOfListOfGetCourierVelocityResponse> GetCourierVelocitiesByCourierIdAsync(HttpRequest request, Guid courierId)
        {
            _settingClient.SetAuthorization(request.GetAuthorization());
            _settingClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _settingClient.Couriers_GetVelocityByCourierIdAsync (courierId);
        }

        public async Task<ResponseOfListOfGetCourierPaymentResponse> GetCourierPaymentsByCourierIdAsync(HttpRequest request, Guid courierId)
        {
            _settingClient.SetAuthorization(request.GetAuthorization());
            _settingClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _settingClient.Couriers_GetPaymentByCourierIdAsync(courierId);
        }

        public async Task<ResponseOfListOfGetCourierInsuranceResponse> GetCourierInsurancesByCourierIdAsync(HttpRequest request, Guid courierId)
        {
            _settingClient.SetAuthorization(request.GetAuthorization());
            _settingClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _settingClient.Couriers_GetInsuranceByCourierIdAsync(courierId);
        }

        public async Task<ResponseOfGetShippingResponse> GetShippingByChannelIdAsync(HttpRequest request, Guid channelId)
        {
            _settingClient.SetAuthorization(request.GetAuthorization());
            _settingClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _settingClient.Shippings_GetByFilterAsync(channelId);
        }

        public async Task<ResponseOfGetPaymentResponse> GetPaymentByChannelIdAsync(HttpRequest request, Guid channelId)
        {
            _settingClient.SetAuthorization(request.GetAuthorization());
            _settingClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _settingClient.Payments_GetByFilterAsync(channelId);
        }

        public async Task<ResponseOfGetPreferencesResponse> GetPreferencesByChannelIdAsync(HttpRequest request, Guid channelId)
        {
            _settingClient.SetAuthorization(request.GetAuthorization());
            _settingClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _settingClient.Preferences_GetByFilterAsync(channelId);
        }
    }
}