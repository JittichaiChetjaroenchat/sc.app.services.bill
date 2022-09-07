using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Customer.Client;

namespace SC.App.Services.Bill.Client.Customer
{
    public interface ICustomerManager
    {
        Task<ResponseOfGetCustomerResponse> GetCustomerByIdAsync(HttpRequest request, Guid id);

        Task<ResponseOfCreateCustomerResponse> CreateCustomerAsync(HttpRequest request, Guid channelId, string name, string address, Guid? areaId, string primaryPhone, string secondaryPhone, string email, Guid userId);

        Task<ResponseOfUpdateCustomerContactResponse> UpdateCustomerContactAsync(HttpRequest request, Guid customerId, string address, Guid? areaId, string primaryPhone, string secondaryPhone, string email);

        Task<ResponseOfRegularCustomerResponse> RegularCustomerAsync(HttpRequest request, Guid customerId);
    }
}