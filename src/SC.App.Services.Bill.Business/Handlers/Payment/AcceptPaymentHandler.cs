using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Payment;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Payment
{
    public class AcceptPaymentHandler : BaseHandler, IRequestHandler<AcceptPaymentCommand, Response<AcceptPaymentResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentManager _paymentManager;

        public AcceptPaymentHandler(
            IConfiguration configuration,
            IPaymentManager paymentManager) : base(configuration)
        {
            _configuration = configuration;
            _paymentManager = paymentManager;
        }

        public async Task<Response<AcceptPaymentResponse>> Handle(AcceptPaymentCommand command, CancellationToken cancellationToken)
        {
            return await _paymentManager.UpdateAsync(_configuration, command);
        }
    }
}