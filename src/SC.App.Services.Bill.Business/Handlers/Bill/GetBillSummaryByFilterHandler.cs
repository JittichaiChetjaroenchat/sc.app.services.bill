using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Queries.Bill;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Bill
{
    public class GetBillSummaryByFilterHandler : BaseHandler, IRequestHandler<GetBillSummaryByFilterQuery, Response<GetBillSummaryResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IBillManager _billManager;

        public GetBillSummaryByFilterHandler(
            IConfiguration configuration,
            IBillManager billManager) : base(configuration)
        {
            _configuration = configuration;
            _billManager = billManager;
        }

        public async Task<Response<GetBillSummaryResponse>> Handle(GetBillSummaryByFilterQuery query, CancellationToken cancellationToken)
        {
            return await _billManager.GetAsync(_configuration, query);
        }
    }
}