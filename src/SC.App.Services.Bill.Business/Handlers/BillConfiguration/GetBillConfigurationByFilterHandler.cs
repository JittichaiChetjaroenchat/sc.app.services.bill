using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Queries.BillConfiguration;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.BillConfiguration
{
    public class GetBillConfigurationByFilterHandler : BaseHandler, IRequestHandler<GetBillConfigurationByFilterQuery, Response<GetBillConfigurationResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IBillConfigurationManager _billConfigurationManager;

        public GetBillConfigurationByFilterHandler(
            IConfiguration configuration,
            IBillConfigurationManager billConfigurationManager) : base(configuration)
        {
            _configuration = configuration;
            _billConfigurationManager = billConfigurationManager;
        }

        public async Task<Response<GetBillConfigurationResponse>> Handle(GetBillConfigurationByFilterQuery query, CancellationToken cancellationToken)
        {
            return await _billConfigurationManager.GetAsync(_configuration, query);
        }
    }
}