using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class BillRecipientContactHelper
    {
        public static string GetFullAddress(string address, Area.Client.GetAreaResponse area)
        {
            if (address.IsEmpty() ||
                area == null)
            {
                return null;
            }

            return $"{address} {area.Sub_district} {area.District} {area.Province} {area.Postal_code}";
        }
    }
}