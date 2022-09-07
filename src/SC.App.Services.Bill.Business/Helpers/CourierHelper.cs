using System;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Business.Helpers
{
    public class CourierHelper
    {
        public static Courier.Client.EnumCourier GetByCode(string code)
        {
            if (code.IsEmpty())
            {
                return Courier.Client.EnumCourier.Unknown;
            }

            if (code.Equals(EnumCourier.Flash.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                return Courier.Client.EnumCourier.Flash;
            }
            else if (code.Equals(EnumCourier.JT.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                return Courier.Client.EnumCourier.JT;
            }
            else if (code.Equals(EnumCourier.ThailandPost.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                return Courier.Client.EnumCourier.ThailandPost;
            }
            else if (code.Equals(EnumCourier.SCG.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                return Courier.Client.EnumCourier.SCG;
            }

            return Courier.Client.EnumCourier.Unknown;
        }
    }
}