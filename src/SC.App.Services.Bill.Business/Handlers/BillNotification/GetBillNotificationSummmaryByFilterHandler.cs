using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Queries.BillNotification;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.BillNotification
{
    public class GetBillNotificationSummmaryByFilterHandler : BaseHandler, IRequestHandler<GetBillNotificationSummaryByFilterQuery, Response<GetBillNotificationSummaryResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IBillNotificationManager _billNotificationManager;

        public GetBillNotificationSummmaryByFilterHandler(
            IConfiguration configuration,
            IBillNotificationManager billNotificationManager) : base(configuration)
        {
            _configuration = configuration;
            _billNotificationManager = billNotificationManager;
        }

        public async Task<Response<GetBillNotificationSummaryResponse>> Handle(GetBillNotificationSummaryByFilterQuery query, CancellationToken cancellationToken)
        {
            return await _billNotificationManager.GetAsync(_configuration, query);
        }
    }
}