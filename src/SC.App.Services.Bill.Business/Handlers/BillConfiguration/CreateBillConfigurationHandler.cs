using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.BillConfiguration;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.BillConfiguration
{
    public class CreateBillConfigurationHandler : BaseHandler, IRequestHandler<CreateBillConfigurationCommand, Response<CreateBillConfigurationResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IBillConfigurationManager _billConfigurationManager;

        public CreateBillConfigurationHandler(
            IConfiguration configuration,
            IBillConfigurationManager billConfigurationManager) : base(configuration)
        {
            _configuration = configuration;
            _billConfigurationManager = billConfigurationManager;
        }

        public async Task<Response<CreateBillConfigurationResponse>> Handle(CreateBillConfigurationCommand command, CancellationToken cancellationToken)
        {
            return await _billConfigurationManager.CreateAsync(_configuration, command);
        }
    }
}