using System;
using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Database.Models;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class PaymentHelper
    {
        public static ICollection<Payment> GetPayments(ICollection<Payment> payments, string[] statuses)
        {
            return payments
                .Where(x => statuses.Contains(x.PaymentStatus.Code))
                .ToList();
        }

        public static Payment GetLatest(ICollection<Payment> payments)
        {
            return payments
                .OrderByDescending(x => x.PaymentNo)
                .FirstOrDefault();
        }

        public static List<Payment> GetAccepted(ICollection<Payment> payments)
        {
            return payments
                .Where(x => x.PaymentStatus.Code == EnumPaymentStatus.Accepted.GetDescription())
                .ToList();
        }
    }
}