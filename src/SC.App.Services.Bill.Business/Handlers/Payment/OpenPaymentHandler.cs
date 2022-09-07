using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Payment;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Payment
{
    public class OpenPaymentHandler : BaseHandler, IRequestHandler<OpenPaymentCommand, Response<OpenPaymentResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentManager _paymentManager;

        public OpenPaymentHandler(
            IConfiguration configuration,
            IPaymentManager paymentManager) : base(configuration)
        {
            _configuration = configuration;
            _paymentManager = paymentManager;
        }

        public async Task<Response<OpenPaymentResponse>> Handle(OpenPaymentCommand command, CancellationToken cancellationToken)
        {
            return await _paymentManager.UpdateAsync(_configuration, command);
        }
    }
}