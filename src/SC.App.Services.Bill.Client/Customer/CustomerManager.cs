using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Bill.Common.Extensions;
using SC.App.Services.Bill.Lib.Extensions;
using SC.App.Services.Customer.Client;

namespace SC.App.Services.Bill.Client.Customer
{
    public class CustomerManager : ICustomerManager
    {
        private readonly ICustomerClient _customerClient;

        public CustomerManager(
            ICustomerClient customerClient)
        {
            _customerClient = customerClient;
        }

        public async Task<ResponseOfGetCustomerResponse> GetCustomerByIdAsync(HttpRequest request, Guid id)
        {
            _customerClient.SetAuthorization(request.GetAuthorization());
            _customerClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _customerClient.Customers_GetByIdAsync(id);
        }

        public async Task<ResponseOfCreateCustomerResponse> CreateCustomerAsync(HttpRequest request, Guid channelId, string name, string address, Guid? areaId, string primaryPhone, string secondaryPhone, string email, Guid userId)
        {
            _customerClient.SetAuthorization(request.GetAuthorization());
            _customerClient.SetAcceptLanguage(request.GetAcceptLanguage());

            CreateContact contact = null;
            if (!address.IsEmpty())
            {
                contact = new CreateContact
                {
                    Address = address,
                    Area_id = areaId,
                    Primary_phone = primaryPhone,
                    Secondary_phone = secondaryPhone,
                    Email = email
                };
            }
            var payload = new CreateCustomer
            {
                Channel_id = channelId,
                Name = name,
                Contact = contact
            };

            return await _customerClient.Customers_CreateAsync(payload);
        }

        public async Task<ResponseOfUpdateCustomerContactResponse> UpdateCustomerContactAsync(HttpRequest request, Guid customerId, string address, Guid? areaId, string primaryPhone, string secondaryPhone, string email)
        {
            _customerClient.SetAuthorization(request.GetAuthorization());
            _customerClient.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new UpdateCustomerContact
            {
                Customer_id = customerId,
                Address = address,
                Area_id = areaId,
                Primary_phone = primaryPhone,
                Secondary_phone = secondaryPhone,
                Email = email
            };

            return await _customerClient.CustomerContacts_UpdateAsync(payload);
        }

        public async Task<ResponseOfRegularCustomerResponse> RegularCustomerAsync(HttpRequest request, Guid customerId)
        {
            _customerClient.SetAuthorization(request.GetAuthorization());
            _customerClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _customerClient.Customers_RegularAsync(customerId);
        }
    }
}