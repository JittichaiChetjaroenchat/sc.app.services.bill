using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.BillConfiguration;
using SC.App.Services.Bill.Business.Queries.BillConfiguration;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Managers.Interface
{
    public interface IBillConfigurationManager
    {
        Task<Response<GetBillConfigurationResponse>> GetAsync(IConfiguration configuration, GetBillConfigurationByFilterQuery query);

        Task<Response<CreateBillConfigurationResponse>> CreateAsync(IConfiguration configuration, CreateBillConfigurationCommand command);

        void IncreaseCurrentNo(Guid channelId);
    }
}