using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class CodHelper
    {
        public static decimal Calculate(decimal total, BillPayment billPayment)
        {
            if (!billPayment.HasCodAddOn)
            {
                return 0;
            }

            if (billPayment.CodAddOnAmount.HasValue)
            {
                return billPayment.CodAddOnAmount.Value;
            }

            if (billPayment.CodAddOnPercentage.HasValue)
            {
                var percentage = billPayment.CodAddOnPercentage.Value;

                return total * percentage / 100;
            }

            return 0;
        }
    }
}