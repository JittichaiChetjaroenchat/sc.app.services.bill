using SC.App.Services.Courier.Client;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class ParcelHelper
    {
        public static decimal GetCodAmount(decimal total, EnumPaymentType paymentType)
        {
            return paymentType == EnumPaymentType.Cod ? total : 0;
        }

        public static decimal GetInsuranceAmount(decimal total, EnumInsuranceType insuranceType)
        {
            return insuranceType == EnumInsuranceType.Yes ? total : 0;
        }
    }
}