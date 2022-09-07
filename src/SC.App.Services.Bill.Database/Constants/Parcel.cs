namespace SC.App.Services.Bill.Database.Constants
{
    public class Parcel
    {
        public const string TableName = "parcels";

        public static class Column
        {
            public const string Id = "id";

            public const string BillId = "bill_id";

            public const string ParcelStatusId = "parcel_status_id";

            public const string Remark = "remark";

            public const string CreatedOn = "created_on";

            public const string IsPrinted = "is_printed";

            public const string IsPacked = "is_packed";
        }
    }
}