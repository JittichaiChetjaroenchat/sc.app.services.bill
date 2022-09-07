using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.PaymentVerification;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.PaymentVerification
{
    public class VerifyPaymentHandler : BaseHandler, IRequestHandler<VerifyPaymentCommand, Response<VerifyPaymentResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentVerificationManager _paymentVerificationManager;

        public VerifyPaymentHandler(
            IConfiguration configuration,
            IPaymentVerificationManager paymentVerificationManager) : base(configuration)
        {
            _configuration = configuration;
            _paymentVerificationManager = paymentVerificationManager;
        }

        public async Task<Response<VerifyPaymentResponse>> Handle(VerifyPaymentCommand command, CancellationToken cancellationToken)
        {
            return await _paymentVerificationManager.CreateAsync(_configuration, command);
        }
    }
}