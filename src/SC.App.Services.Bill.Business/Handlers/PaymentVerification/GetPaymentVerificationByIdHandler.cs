using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Queries.PaymentVerification;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.PaymentVerification
{
    public class GetPaymentVerificationByIdHandler : BaseHandler, IRequestHandler<GetPaymentVerificationByIdQuery, Response<GetPaymentVerificationResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentVerificationManager _paymentVerificationManager;

        public GetPaymentVerificationByIdHandler(
            IConfiguration configuration,
            IPaymentVerificationManager paymentVerificationManager) : base(configuration)
        {
            _configuration = configuration;
            _paymentVerificationManager = paymentVerificationManager;
        }

        public async Task<Response<GetPaymentVerificationResponse>> Handle(GetPaymentVerificationByIdQuery query, CancellationToken cancellationToken)
        {
            return await _paymentVerificationManager.GetAsync(_configuration, query);
        }
    }
}