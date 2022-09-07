using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Tag;
using SC.App.Services.Bill.Business.Queries.Tag;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Managers.Interface
{
    public interface ITagManager
    {
        Task<Response<GetTagResponse>> GetAsync(IConfiguration configuration, GetTagByIdQuery query);

        Task<Response<List<GetTagResponse>>> GetAsync(IConfiguration configuration, GetTagByFilterQuery query);

        Task<Response<CreateTagResponse>> CreateAsync(IConfiguration configuration, CreateTagCommand command);

        Task<Response<UpdateTagResponse>> UpdateAsync(IConfiguration configuration, UpdateTagCommand command);

        Task<Response<DeleteTagByIdResponse>> DeleteAsync(IConfiguration configuration, DeleteTagByIdCommand command);

        Task<Response<DeleteTagByIdsResponse>> DeleteAsync(IConfiguration configuration, DeleteTagByIdsCommand command);
    }
}