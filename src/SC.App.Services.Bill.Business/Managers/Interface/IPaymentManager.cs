using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.Payment;
using SC.App.Services.Bill.Business.Queries.Payment;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Managers.Interface
{
    public interface IPaymentManager
    {
        Task<Response<GetPaymentResponse>> GetAsync(IConfiguration configuration, GetPaymentByIdQuery query);

        Task<Response<List<GetPaymentResponse>>> GetAsync(IConfiguration configuration, GetPaymentByFilterQuery query);

        Task<Response<OpenPaymentResponse>> UpdateAsync(IConfiguration configuration, OpenPaymentCommand command);

        Task<Response<AcceptPaymentResponse>> UpdateAsync(IConfiguration configuration, AcceptPaymentCommand command);

        Task<Response<RejectPaymentResponse>> UpdateAsync(IConfiguration configuration, RejectPaymentCommand command);
    }
}