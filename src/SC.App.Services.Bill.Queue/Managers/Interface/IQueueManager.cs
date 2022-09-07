using System;
using System.Threading.Tasks;

namespace SC.App.Services.Bill.Queue.Managers.Interface
{
    public interface IQueueManager
    {
        Task NotifyPaymentAcceptAsync(Guid paymentId);

        Task NotifyPaymentRejectAsync(Guid paymentId);

        Task NotifyDeliveryAddressAcceptAsync(Guid billId);

        Task NotifyDeliveryAddressRejectAsync(Guid billId);

        Task NotifyBillConfirmAsync(Guid billId);

        Task NotifyBillCancelAsync(Guid billId);

        Task NotifyBillSummaryAsync(Guid billId);

        Task NotifyParcelIssueAsync(Guid parcelId);

        Task NotifyOrderConfirmAsync(Guid orderId);

        Task VerifyPaymentAsync(Guid paymentId);
    }
}