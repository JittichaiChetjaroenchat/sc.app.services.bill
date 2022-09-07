using System;
using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class CourierSettingHelper
    {
        public static Courier.Client.EnumShippingType GetShippingType(string code)
        {
            if (code.IsEmpty())
            {
                return Courier.Client.EnumShippingType.Unknown;
            }

            if (code.Equals(Courier.Client.EnumShippingType.Pickup.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Courier.Client.EnumShippingType.Pickup;
            }
            else if (code.Equals(Courier.Client.EnumShippingType.DropOff.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Courier.Client.EnumShippingType.DropOff;
            }

            return Courier.Client.EnumShippingType.Unknown;
        }

        public static Courier.Client.EnumVelocityType GetVelocityType(string code)
        {
            if (code.IsEmpty())
            {
                return Courier.Client.EnumVelocityType.Unknown;
            }

            if (code.Equals(Courier.Client.EnumVelocityType.Standard.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Courier.Client.EnumVelocityType.Standard;
            }
            else if (code.Equals(Courier.Client.EnumVelocityType.Express.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Courier.Client.EnumVelocityType.Express;
            }

            return Courier.Client.EnumVelocityType.Unknown;
        }

        public static Courier.Client.EnumPaymentType GetPaymentType(string code)
        {
            if (code.IsEmpty())
            {
                return Courier.Client.EnumPaymentType.Unknown;
            }

            if (code.Equals(Courier.Client.EnumPaymentType.Standard.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Courier.Client.EnumPaymentType.Standard;
            }
            else if (code.Equals(Courier.Client.EnumPaymentType.Cod.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Courier.Client.EnumPaymentType.Cod;
            }

            return Courier.Client.EnumPaymentType.Unknown;
        }

        public static Courier.Client.EnumPaymentType GetPaymentType(EnumBillPaymentType billPaymentType)
        {
            switch (billPaymentType)
            {
                case EnumBillPaymentType.PrePaid:
                    return Courier.Client.EnumPaymentType.Standard;
                case EnumBillPaymentType.PostPaid:
                    return Courier.Client.EnumPaymentType.Cod;
                default:
                    return Courier.Client.EnumPaymentType.Unknown;
            }
        }

        public static Courier.Client.EnumInsuranceType GetInsuranceType(string code)
        {
            if (code.IsEmpty())
            {
                return Courier.Client.EnumInsuranceType.Unknown;
            }

            if (code.Equals(Courier.Client.EnumInsuranceType.No.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Courier.Client.EnumInsuranceType.No;
            }
            else if (code.Equals(Courier.Client.EnumInsuranceType.Yes.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return Courier.Client.EnumInsuranceType.Yes;
            }

            return Courier.Client.EnumInsuranceType.Unknown;
        }

        public static Setting.Client.GetCourierPaymentResponse GetPaymentType(ICollection<Setting.Client.GetCourierPaymentResponse> payments, Courier.Client.EnumPaymentType paymentType)
        {
            var payment = payments
                .FirstOrDefault(x => x.Code.Equals(paymentType.ToString(), StringComparison.OrdinalIgnoreCase));

            return payment;
        }

        public static Setting.Client.GetCourierInsuranceResponse GetInsuranceType(ICollection<Setting.Client.GetCourierInsuranceResponse> insurances, Courier.Client.EnumInsuranceType insuranceType)
        {
            var insurance = insurances
                .FirstOrDefault(x => x.Code.Equals(insuranceType.ToString(), StringComparison.OrdinalIgnoreCase));

            return insurance;
        }

        public static Setting.Client.GetCourierInsuranceResponse GetInsuranceType(ICollection<Setting.Client.GetCourierInsuranceResponse> insurances, Guid insuranceTypeId)
        {
            var insurance = insurances
                .FirstOrDefault(x => x.Courier_insurance_type_id == insuranceTypeId);

            return insurance;
        }
    }
}