using SC.App.Services.Bill.Database.Models;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class DiscountHelper
    {
        public static decimal Calculate(decimal cost, BillDiscount discount)
        {
            if (discount != null)
            {
                // Calculate by amount
                if (discount.Amount.HasValue)
                {
                    return discount.Amount.Value;
                }

                // Calculate by percentage
                if (discount.Percentage.HasValue)
                {
                    return (cost * discount.Percentage.Value) / 100;
                }
            }
            
            return 0;
        }
    }
}