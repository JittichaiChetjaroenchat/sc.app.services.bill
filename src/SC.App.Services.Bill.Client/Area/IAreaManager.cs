using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Area.Client;

namespace SC.App.Services.Bill.Client.Area
{
    public interface IAreaManager
    {
        Task<ResponseOfListOfGetAreaResponse> GetAreasAsync(HttpRequest request);

        Task<ResponseOfGetAreaResponse> GetAreaByIdAsync(HttpRequest request, Guid id);
    }
}