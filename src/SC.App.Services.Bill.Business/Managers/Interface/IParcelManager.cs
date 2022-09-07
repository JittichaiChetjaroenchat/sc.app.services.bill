using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Parcel;
using SC.App.Services.Bill.Business.Queries.Parcel;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Managers.Interface
{
    public interface IParcelManager
    {
        Task<Response<GetParcelResponse>> GetAsync(IConfiguration configuration, GetParcelByIdQuery query);

        Task<Response<List<GetParcelResponse>>> GetAsync(IConfiguration configuration, GetParcelByFilterQuery query);

        Task<Response<CreateParcelResponse>> CreateAsync(IConfiguration configuration, CreateParcelCommand command);

        Task<Response<CreateParcelsResponse>> CreateAsync(IConfiguration configuration, CreateParcelsCommand command);

        Task<Response<UpdateParcelResponse>> UpdateAsync(IConfiguration configuration, UpdateParcelCommand command);

        Task<Response<UpdateParcelPrintedResponse>> UpdateAsync(IConfiguration configuration, UpdateParcelPrintedCommand command);

        Task<Response<CancelParcelResponse>> UpdateAsync(IConfiguration configuration, CancelParcelCommand command);
    }
}