using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.BillNotification;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.BillNotification
{
    public class NotifyBillSummaryHandler : BaseHandler, IRequestHandler<NotifyBillSummaryCommand, Response<NotifyBillSummaryResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IBillNotificationManager _billNotificationManager;

        public NotifyBillSummaryHandler(
            IConfiguration configuration,
            IBillNotificationManager billNotificationManager) : base(configuration)
        {
            _configuration = configuration;
            _billNotificationManager = billNotificationManager;
        }

        public async Task<Response<NotifyBillSummaryResponse>> Handle(NotifyBillSummaryCommand command, CancellationToken cancellationToken)
        {
            return await _billNotificationManager.CreateAsync(_configuration, command);
        }
    }
}