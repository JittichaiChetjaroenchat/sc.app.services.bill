using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Notification.Client;

namespace SC.App.Services.Bill.Client.Notification
{
    public class NotificationClientHelper
    {
        public static bool IsSuccess(ResponseOfCreateNotificationResponse response)
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