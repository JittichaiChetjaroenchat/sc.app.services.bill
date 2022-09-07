using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.PaymentVerification;
using SC.App.Services.Bill.Business.Queries.PaymentVerification;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Managers.Interface
{
    public interface IPaymentVerificationManager
    {
        Task<Response<GetPaymentVerificationResponse>> GetAsync(IConfiguration configuration, GetPaymentVerificationByIdQuery query);

        Task<Response<VerifyPaymentResponse>> CreateAsync(IConfiguration configuration, VerifyPaymentCommand command);
    }
}