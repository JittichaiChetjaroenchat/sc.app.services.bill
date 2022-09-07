using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Area.Client;
using SC.App.Services.Bill.Common.Extensions;

namespace SC.App.Services.Bill.Client.Area
{
    public class AreaManager : IAreaManager
    {
        private readonly IAreaClient _areaClient;

        public AreaManager(
            IAreaClient areaClient)
        {
            _areaClient = areaClient;
        }

        public async Task<ResponseOfGetAreaResponse> GetAreaByIdAsync(HttpRequest request, Guid id)
        {
            _areaClient.SetAuthorization(request.GetAuthorization());
            _areaClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _areaClient.Areas_GetByIdAsync(id);
        }

        public async Task<ResponseOfListOfGetAreaResponse> GetAreasAsync(HttpRequest request)
        {
            _areaClient.SetAuthorization(request.GetAuthorization());
            _areaClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _areaClient.Areas_GetByFilterAsync();
        }
    }
}