using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Queries.Bill;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Bill
{
    public class GetLatestBillByCustomerIdHandler : BaseHandler, IRequestHandler<GetLatestBillByFilterQuery, Response<GetBillResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IBillManager _billManager;

        public GetLatestBillByCustomerIdHandler(
            IConfiguration configuration,
            IBillManager billManager) : base(configuration)
        {
            _configuration = configuration;
            _billManager = billManager;
        }

        public async Task<Response<GetBillResponse>> Handle(GetLatestBillByFilterQuery command, CancellationToken cancellationToken)
        {
            return await _billManager.GetAsync(_configuration, command);
        }
    }
}