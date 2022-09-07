using System;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class BillPaymentTypeHelper
    {
        public static EnumBillPaymentType GetByCode(string code)
        {
            if (code.IsEmpty())
            {
                return EnumBillPaymentType.Unknown;
            }

            if (code.Equals(EnumBillPaymentType.PrePaid.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                return EnumBillPaymentType.PrePaid;
            }
            else if (code.Equals(EnumBillPaymentType.PostPaid.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                return EnumBillPaymentType.PostPaid;
            }

            return EnumBillPaymentType.Unknown;
        }
    }
}