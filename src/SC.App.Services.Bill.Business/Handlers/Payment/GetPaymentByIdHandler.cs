using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Queries.Payment;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Payment
{
    public class GetPaymentByIdHandler : BaseHandler, IRequestHandler<GetPaymentByIdQuery, Response<GetPaymentResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentManager _paymentManager;

        public GetPaymentByIdHandler(
            IConfiguration configuration,
            IPaymentManager paymentManager) : base(configuration)
        {
            _configuration = configuration;
            _paymentManager = paymentManager;
        }

        public async Task<Response<GetPaymentResponse>> Handle(GetPaymentByIdQuery query, CancellationToken cancellationToken)
        {
            return await _paymentManager.GetAsync(_configuration, query);
        }
    }
}