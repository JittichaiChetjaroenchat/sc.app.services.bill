using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Credit.Client;

namespace SC.App.Services.Bill.Client.Credit
{
    public interface ICreditManager
    {
        Task<ResponseOfCheckCreditBalanceAvailableResponse> CheckCreditBalanceAvailableAsync(HttpRequest request, Guid channelId, decimal amount);

        Task<ResponseOfUpdateCreditResponse> UpdateCreditAsync(HttpRequest request, Guid channelId, EnumCreditExpenseType expenseType, decimal amount, string remark, Guid userId);
    }
}