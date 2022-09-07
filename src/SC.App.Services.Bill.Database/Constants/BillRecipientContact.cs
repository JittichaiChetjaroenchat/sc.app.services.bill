namespace SC.App.Services.Bill.Database.Constants
{
    public class BillRecipientContact
    {
        public const string TableName = "bill_recipient_contacts";

        public static class Column
        {
            public const string Id = "id";

            public const string BillRecipientId = "bill_recipient_id";

            public const string Address = "address";

            public const string AreaId = "area_id";

            public const string PrimaryPhone = "primary_phone";

            public const string SecondaryPhone = "secondary_phone";

            public const string Email = "email";

            public const string CreatedBy = "created_by";

            public const string CreatedOn = "created_on";

            public const string UpdatedBy = "updated_by";

            public const string UpdatedOn = "updated_on";
        }
    }
}