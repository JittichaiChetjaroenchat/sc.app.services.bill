using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Security.Client;

namespace SC.App.Services.Bill.Client.Security
{
    public class SecurityClientHelper
    {
        public static bool IsSuccess(ResponseOfValidateAccessTokenResponse response)
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