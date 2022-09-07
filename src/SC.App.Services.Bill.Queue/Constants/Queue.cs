namespace SC.App.Services.Bill.Queue.Constants
{
    public class Queue
    {
        public class Bill
        {
            public const string Queue = "bill";

            public const string Exchange = "bill";

            public const string Type = "topic";

            public class NotifyPaymentAccept
            {
                public const string RoutingKey = "bill.notify_payment_accept";
            }

            public class NotifyPaymentReject
            {
                public const string RoutingKey = "bill.notify_payment_reject";
            }

            public class NotifyDeliveryAddressAccept
            {
                public const string RoutingKey = "bill.notify_delivery_address_accept";
            }

            public class NotifyDeliveryAddressReject
            {
                public const string RoutingKey = "bill.notify_delivery_address_reject";
            }

            public class NotifyBillConfirm
            {
                public const string RoutingKey = "bill.notify_bill_confirm";
            }

            public class NotifyBillCancel
            {
                public const string RoutingKey = "bill.notify_bill_cancel";
            }

            public class NotifyBillSummary
            {
                public const string RoutingKey = "bill.notify_bill_summary";
            }

            public class NotifyParcelIssue
            {
                public const string RoutingKey = "bill.notify_parcel_issue";
            }

            public class VerifyPayment
            {
                public const string RoutingKey = "bill.verify_payment";
            }
        }

        public class Order
        {
            public const string Queue = "order";

            public const string Exchange = "order";

            public const string Type = "topic";

            public class NotifyOrderConfirm
            {
                public const string RoutingKey = "order.notify_order_confirm";
            }
        }
    }
}