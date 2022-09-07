namespace SC.App.Services.Bill.Business.Helpers
{
    public class VatHelper
    {
        public static decimal Calculate(decimal cost, Database.Models.BillPayment billPayment)
        {
            if (billPayment != null &&
                billPayment.HasVat &&
                !billPayment.IncludedVat)
            {
                return (cost * billPayment.VatPercentage) / 100;
            }

            return 0;
        }
    }
}