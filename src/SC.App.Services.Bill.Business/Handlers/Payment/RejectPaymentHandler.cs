using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Payment;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Payment
{
    public class RejectPaymentHandler : BaseHandler, IRequestHandler<RejectPaymentCommand, Response<RejectPaymentResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentManager _paymentManager;

        public RejectPaymentHandler(
            IConfiguration configuration,
            IPaymentManager paymentManager) : base(configuration)
        {
            _configuration = configuration;
            _paymentManager = paymentManager;
        }

        public async Task<Response<RejectPaymentResponse>> Handle(RejectPaymentCommand command, CancellationToken cancellationToken)
        {
            return await _paymentManager.UpdateAsync(_configuration, command);
        }
    }
}