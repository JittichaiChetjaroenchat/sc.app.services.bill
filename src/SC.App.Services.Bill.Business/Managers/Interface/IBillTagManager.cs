using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.BillTag;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Managers.Interface
{
    public interface IBillTagManager
    {
        Task<Response<CreateBillTagResponse>> CreateAsync(IConfiguration configuration, CreateBillTagCommand command);

        Task<Response<UpdateBillTagResponse>> UpdateAsync(IConfiguration configuration, UpdateBillTagCommand command);

        Task<Response<DeleteBillTagResponse>> DeleteAsync(IConfiguration configuration, DeleteBillTagCommand command);
    }
}