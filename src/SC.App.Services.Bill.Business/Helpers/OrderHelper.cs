using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Database.Models;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class OrderHelper
    {
        public static int GetAmount(ICollection<Order.Client.GetOrderResponse> orders)
        {
            return orders
                .Sum(x => x.Amount);
        }

        public static decimal GetPrice(Order.Client.GetOrderResponse order)
        {
            return order.Amount * order.Unit_price;
        }

        public static decimal GetPrice(ICollection<Order.Client.GetOrderResponse> orders)
        {
            return orders
                .Sum(x => x.Amount * x.Unit_price);
        }

        public static decimal GetAcceptedAmount(ICollection<Payment> payments)
        {
            var acceptedPayments = PaymentHelper.GetPayments(payments, new string[] { EnumPaymentStatus.Accepted.GetDescription() });

            return acceptedPayments
                .Sum(x => x.Amount);
        }
    }
}