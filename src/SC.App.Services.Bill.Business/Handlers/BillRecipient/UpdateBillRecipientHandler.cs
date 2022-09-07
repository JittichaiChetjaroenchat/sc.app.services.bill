using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.BillRecipient;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.BillRecipient
{
    public class UpdateBillRecipientHandler : BaseHandler, IRequestHandler<UpdateBillRecipientCommand, Response<UpdateBillRecipientResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IBillRecipientManager _billRecipientManager;

        public UpdateBillRecipientHandler(
            IConfiguration configuration,
            IBillRecipientManager billRecipientnManager) : base(configuration)
        {
            _configuration = configuration;
            _billRecipientManager = billRecipientnManager;
        }

        public async Task<Response<UpdateBillRecipientResponse>> Handle(UpdateBillRecipientCommand command, CancellationToken cancellationToken)
        {
            return await _billRecipientManager.UpdateAsync(_configuration, command);
        }
    }
}