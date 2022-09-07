using SC.App.Services.Setting.Client;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class OwnershipTypeHelper
    {
        public static Courier.Client.EnumOwnershipType Get(GetCourierAccountResponse courierAccount)
        {
            return courierAccount.Is_platform_owner ? Courier.Client.EnumOwnershipType.Platform : Courier.Client.EnumOwnershipType.Customer;
        }
    }
}