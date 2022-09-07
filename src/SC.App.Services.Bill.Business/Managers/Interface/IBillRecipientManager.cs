using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.BillRecipient;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Managers.Interface
{
    public interface IBillRecipientManager
    {
        Task<Response<UpdateBillRecipientResponse>> UpdateAsync(IConfiguration configuration, UpdateBillRecipientCommand command);
    }
}