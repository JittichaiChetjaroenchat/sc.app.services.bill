using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Bill;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Bill
{
    public class DepositBillHandler : BaseHandler, IRequestHandler<DepositBillCommand, Response<DepositBillResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IBillManager _billManager;

        public DepositBillHandler(
            IConfiguration configuration,
            IBillManager billManager) : base(configuration)
        {
            _configuration = configuration;
            _billManager = billManager;
        }

        public async Task<Response<DepositBillResponse>> Handle(DepositBillCommand command, CancellationToken cancellationToken)
        {
            return await _billManager.UpdateAsync(_configuration, command);
        }
    }
}