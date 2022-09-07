namespace SC.App.Services.Bill.Database.Constants
{
    public class BillPayment
    {
        public const string TableName = "bill_payments";

        public static class Column
        {
            public const string Id = "id";

            public const string BillId = "bill_id";

            public const string BillPaymentTypeId = "bill_payment_type_id";

            public const string HasCodAddOn = "has_cod_addon";

            public const string CodAddOnAmount = "cod_addon_amount";

            public const string CodAddOnPercentage = "cod_addon_percentage";

            public const string HasVat = "has_vat";

            public const string IncludedVat = "included_vat";

            public const string VatPercentage = "vat_percentage";

            public const string CreatedBy = "created_by";

            public const string CreatedOn = "created_on";

            public const string UpdatedBy = "updated_by";

            public const string UpdatedOn = "updated_on";
        }
    }
}