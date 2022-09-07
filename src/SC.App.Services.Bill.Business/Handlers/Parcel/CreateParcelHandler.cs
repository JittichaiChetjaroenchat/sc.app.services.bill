using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Parcel;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Parcel
{
    public class CreateParcelHandler : BaseHandler, IRequestHandler<CreateParcelCommand, Response<CreateParcelResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IParcelManager _parcelManager;

        public CreateParcelHandler(
            IConfiguration configuration,
            IParcelManager parcelManager) : base(configuration)
        {
            _configuration = configuration;
            _parcelManager = parcelManager;
        }

        public async Task<Response<CreateParcelResponse>> Handle(CreateParcelCommand command, CancellationToken cancellationToken)
        {
            return await _parcelManager.CreateAsync(_configuration, command);
        }
    }
}