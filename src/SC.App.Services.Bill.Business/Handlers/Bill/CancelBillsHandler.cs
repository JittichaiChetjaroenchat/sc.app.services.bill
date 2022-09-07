using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Bill;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Bill
{
    public class CancelBillsHandler : BaseHandler, IRequestHandler<CancelBillsCommand, Response<CancelBillsResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IBillManager _billManager;

        public CancelBillsHandler(
            IConfiguration configuration,
            IBillManager billManager) : base(configuration)
        {
            _configuration = configuration;
            _billManager = billManager;
        }

        public async Task<Response<CancelBillsResponse>> Handle(CancelBillsCommand command, CancellationToken cancellationToken)
        {
            return await _billManager.UpdateAsync(_configuration, command);
        }
    }
}