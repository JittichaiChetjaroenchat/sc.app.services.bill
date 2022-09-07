using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Credit.Client;

namespace SC.App.Services.Bill.Client.Credit
{
    public class CreditClientHelper
    {
        public static bool IsSuccess(ResponseOfGetCreditResponse response)
        {
            if (response == null)
            {
                return false;
            }

            return IsOk(response.Status);
        }

        public static bool IsSuccess(ResponseOfCheckCreditBalanceAvailableResponse response)
        {
            if (response == null)
            {
                return false;
            }

            return IsOk(response.Status);
        }

        public static bool IsSuccess(ResponseOfUpdateCreditResponse response)
        {
            if (response == null)
            {
                return false;
            }

            return IsOk(response.Status);
        }

        public static ResponseError GetError(ICollection<ResponseError> errors)
        {
            return errors.FirstOrDefault();
        }

        private static bool IsOk(EnumResponseStatus status)
        {
            return status == EnumResponseStatus.OK;
        }
    }
}