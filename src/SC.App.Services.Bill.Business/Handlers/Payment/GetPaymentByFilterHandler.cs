using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Queries.Payment;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Payment
{
    public class GetPaymentByFilterHandler : BaseHandler, IRequestHandler<GetPaymentByFilterQuery, Response<List<GetPaymentResponse>>>
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentManager _paymentManager;

        public GetPaymentByFilterHandler(
            IConfiguration configuration,
            IPaymentManager paymentManager) : base(configuration)
        {
            _configuration = configuration;
            _paymentManager = paymentManager;
        }

        public async Task<Response<List<GetPaymentResponse>>> Handle(GetPaymentByFilterQuery query, CancellationToken cancellationToken)
        {
            return await _paymentManager.GetAsync(_configuration, query);
        }
    }
}