using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.BillTag;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.BillTag
{
    public class DeleteBillTagHandler : BaseHandler, IRequestHandler<DeleteBillTagCommand, Response<DeleteBillTagResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IBillTagManager _billTagManager;

        public DeleteBillTagHandler(
            IConfiguration configuration,
            IBillTagManager billTagManager) : base(configuration)
        {
            _configuration = configuration;
            _billTagManager = billTagManager;
        }

        public async Task<Response<DeleteBillTagResponse>> Handle(DeleteBillTagCommand command, CancellationToken cancellationToken)
        {
            return await _billTagManager.DeleteAsync(_configuration, command);
        }
    }
}