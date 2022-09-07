using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Bill;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Bill
{
    public class CreateOnlineBillHandler : BaseHandler, IRequestHandler<CreateOnlineBillCommand, Response<CreateBillResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IOnlineBillManager _onlineBillManager;

        public CreateOnlineBillHandler(
            IConfiguration configuration,
            IOnlineBillManager onlineBillManager) : base(configuration)
        {
            _configuration = configuration;
            _onlineBillManager = onlineBillManager;
        }

        public async Task<Response<CreateBillResponse>> Handle(CreateOnlineBillCommand command, CancellationToken cancellationToken)
        {
            return await _onlineBillManager.CreateAsync(_configuration, command);
        }
    }
}