using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Bill;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Managers.Interface
{
    public interface IOnlineBillManager
    {
        Task<Response<CreateBillResponse>> CreateAsync(IConfiguration configuration, CreateOnlineBillCommand command);
    }
}