using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Bill.Common.Extensions;
using SC.App.Services.Credit.Client;

namespace SC.App.Services.Bill.Client.Credit
{
    public class CreditManager : ICreditManager
    {
        private readonly ICreditClient _creditClient;

        public CreditManager(
            ICreditClient creditClient)
        {
            _creditClient = creditClient;
        }

        public async Task<ResponseOfCheckCreditBalanceAvailableResponse> CheckCreditBalanceAvailableAsync(HttpRequest request, Guid channelId, decimal amount)
        {
            _creditClient.SetAuthorization(request.GetAuthorization());
            _creditClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _creditClient.Credits_CheckBalanceAvailableAsync(channelId, amount);
        }

        public async Task<ResponseOfUpdateCreditResponse> UpdateCreditAsync(HttpRequest request, Guid channelId, EnumCreditExpenseType expenseType, decimal amount, string remark, Guid userId)
        {
            _creditClient.SetAuthorization(request.GetAuthorization());
            _creditClient.SetAcceptLanguage(request.GetAcceptLanguage());

            var payload = new UpdateCredit
            {
                Channel_id = channelId,
                Expense_type = expenseType,
                Amount = amount,
                Remark = remark,
                User_id = userId
            };
            return await _creditClient.Credits_UpdateAsync(payload);
        }
    }
}