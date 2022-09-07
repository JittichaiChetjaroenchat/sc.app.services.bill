using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Commands.BillNotification;
using SC.App.Services.Bill.Business.Queries.BillNotification;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Managers.Interface
{
    public interface IBillNotificationManager
    {
        Task<Response<GetBillNotificationResponse>> GetAsync(IConfiguration configuration, GetBillNotificationByIdQuery query);

        Task<Response<SearchBillNotificationResponse>> GetAsync(IConfiguration configuration, SearchBillNotificationByFilterQuery query);

        Task<Response<GetBillNotificationSummaryResponse>> GetAsync(IConfiguration configuration, GetBillNotificationSummaryByFilterQuery query);

        Task<Response<NotifyBillSummaryResponse>> CreateAsync(IConfiguration configuration, NotifyBillSummaryCommand command);
    }
}