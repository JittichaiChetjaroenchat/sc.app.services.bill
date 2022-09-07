using System;
using System.Threading.Tasks;
using SC.App.Services.Bill.Queue.Managers.Interface;
using SC.App.Services.Bill.Queue.Models.Bill;
using SC.App.Services.Bill.Queue.Models.Order;
using SC.App.Services.Bill.Queue.Providers.Interface;
using Serilog;

namespace SC.App.Services.Bill.Queue.Managers
{
    public class QueueManager : IQueueManager
    {
        private readonly IQueueProvider _queueProvider;

        public QueueManager(
            IQueueProvider queueProvider)
        {
            _queueProvider = queueProvider;
        }

        public async Task NotifyPaymentAcceptAsync(Guid paymentId)
        {
            try
            {
                // Publish
                var payload = new NotifyPaymentAccept(paymentId);
                _queueProvider.Publish(Constants.Queue.Bill.Queue, Constants.Queue.Bill.Exchange, Constants.Queue.Bill.NotifyPaymentAccept.RoutingKey, payload);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
            }

            await Task.CompletedTask;
        }

        public async Task NotifyPaymentRejectAsync(Guid paymentId)
        {
            try
            {
                // Publish
                var payload = new NotifyPaymentAccept(paymentId);
                _queueProvider.Publish(Constants.Queue.Bill.Queue, Constants.Queue.Bill.Exchange, Constants.Queue.Bill.NotifyPaymentReject.RoutingKey, payload);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
            }

            await Task.CompletedTask;
        }

        public async Task NotifyDeliveryAddressAcceptAsync(Guid billId)
        {
            try
            {
                // Publish
                var payload = new NotifyDeliveryAddressAccept(billId);
                _queueProvider.Publish(Constants.Queue.Bill.Queue, Constants.Queue.Bill.Exchange, Constants.Queue.Bill.NotifyDeliveryAddressAccept.RoutingKey, payload);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
            }

            await Task.CompletedTask;
        }

        public async Task NotifyDeliveryAddressRejectAsync(Guid billId)
        {
            try
            {
                // Publish
                var payload = new NotifyDeliveryAddressReject(billId);
                _queueProvider.Publish(Constants.Queue.Bill.Queue, Constants.Queue.Bill.Exchange, Constants.Queue.Bill.NotifyDeliveryAddressReject.RoutingKey, payload);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
            }

            await Task.CompletedTask;
        }

        public async Task NotifyBillConfirmAsync(Guid billId)
        {
            try
            {
                // Publish
                var payload = new NotifyBillConfirm(billId);
                _queueProvider.Publish(Constants.Queue.Bill.Queue, Constants.Queue.Bill.Exchange, Constants.Queue.Bill.NotifyBillConfirm.RoutingKey, payload);

            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
            }

            await Task.CompletedTask;
        }

        public async Task NotifyBillCancelAsync(Guid billId)
        {
            try
            {
                // Publish
                var payload = new NotifyBillCancel(billId);
                _queueProvider.Publish(Constants.Queue.Bill.Queue, Constants.Queue.Bill.Exchange, Constants.Queue.Bill.NotifyBillCancel.RoutingKey, payload);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
            }

            await Task.CompletedTask;
        }

        public async Task NotifyBillSummaryAsync(Guid billId)
        {
            try
            {
                // Publish
                var payload = new NotifyBillSummary(billId);
                _queueProvider.Publish(Constants.Queue.Bill.Queue, Constants.Queue.Bill.Exchange, Constants.Queue.Bill.NotifyBillSummary.RoutingKey, payload);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
            }

            await Task.CompletedTask;
        }

        public async Task NotifyParcelIssueAsync(Guid parcelId)
        {
            try
            {
                // Publish
                var payload = new NotifyParcelIssue(parcelId);
                _queueProvider.Publish(Constants.Queue.Bill.Queue, Constants.Queue.Bill.Exchange, Constants.Queue.Bill.NotifyParcelIssue.RoutingKey, payload);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
            }

            await Task.CompletedTask;
        }

        public async Task NotifyOrderConfirmAsync(Guid orderId)
        {
            try
            {
                // Publish
                var payload = new NotifyOrderConfirm(orderId, null);
                _queueProvider.Publish(Constants.Queue.Order.Queue, Constants.Queue.Order.Exchange, Constants.Queue.Order.NotifyOrderConfirm.RoutingKey, payload);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
            }

            await Task.CompletedTask;
        }

        public async Task VerifyPaymentAsync(Guid paymentId)
        {
            try
            {
                // Publish
                var payload = new VerifyPayment(paymentId);
                _queueProvider.Publish(Constants.Queue.Bill.Queue, Constants.Queue.Bill.Exchange, Constants.Queue.Bill.VerifyPayment.RoutingKey, payload);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
            }

            await Task.CompletedTask;
        }
    }
}