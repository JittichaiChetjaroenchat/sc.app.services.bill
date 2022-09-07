using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.BillTag;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.BillTag
{
    public class CreateBillTagHandler : BaseHandler, IRequestHandler<CreateBillTagCommand, Response<CreateBillTagResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IBillTagManager _billTagManager;

        public CreateBillTagHandler(
            IConfiguration configuration,
            IBillTagManager billTagManager) : base(configuration)
        {
            _configuration = configuration;
            _billTagManager = billTagManager;
        }

        public async Task<Response<CreateBillTagResponse>> Handle(CreateBillTagCommand command, CancellationToken cancellationToken)
        {
            return await _billTagManager.CreateAsync(_configuration, command);
        }
    }
}