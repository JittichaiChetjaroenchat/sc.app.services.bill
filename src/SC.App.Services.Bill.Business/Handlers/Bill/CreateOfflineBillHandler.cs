using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Bill;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Bill
{
    public class CreateOfflineBillHandler : BaseHandler, IRequestHandler<CreateOfflineBillCommand, Response<CreateBillResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IOfflineBillManager _offlineBillManager;

        public CreateOfflineBillHandler(
            IConfiguration configuration,
            IOfflineBillManager offlineBillManager) : base(configuration)
        {
            _configuration = configuration;
            _offlineBillManager = offlineBillManager;
        }

        public async Task<Response<CreateBillResponse>> Handle(CreateOfflineBillCommand command, CancellationToken cancellationToken)
        {
            return await _offlineBillManager.CreateAsync(_configuration, command);
        }
    }
}