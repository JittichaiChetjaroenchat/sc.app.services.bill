using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Managers.Interface;
using SC.App.Services.Bill.Business.Queries.Parcel;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Handlers.Parcel
{
    public class GetParcelByIdHandler : BaseHandler, IRequestHandler<GetParcelByIdQuery, Response<GetParcelResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly IParcelManager _parcelManager;

        public GetParcelByIdHandler(
            IConfiguration configuration,
            IParcelManager parcelManager) : base(configuration)
        {
            _configuration = configuration;
            _parcelManager = parcelManager;
        }

        public async Task<Response<GetParcelResponse>> Handle(GetParcelByIdQuery query, CancellationToken cancellationToken)
        {
            return await _parcelManager.GetAsync(_configuration, query);
        }
    }
}