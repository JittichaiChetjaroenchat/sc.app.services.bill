using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Bill;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Bill
{
    public class UpdateOfflineBillHandler : BaseHandler, IRequestHandler<UpdateOfflineBillCommand, Response<UpdateBillResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IOfflineBillManager _offlineBillManager;

        public UpdateOfflineBillHandler(
            IConfiguration configuration,
            IOfflineBillManager offlineBillManager) : base(configuration)
        {
            _configuration = configuration;
            _offlineBillManager = offlineBillManager;
        }

        public async Task<Response<UpdateBillResponse>> Handle(UpdateOfflineBillCommand command, CancellationToken cancellationToken)
        {
            return await _offlineBillManager.UpdateAsync(_configuration, command);
        }
    }
}