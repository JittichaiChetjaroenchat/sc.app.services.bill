using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Streaming.Client;

namespace SC.App.Services.Bill.Client.Streaming
{
    public class StreamingClientHelper
    {
        public static bool IsSuccess(ResponseOfGetLiveResponse response)
        {
            if (response == null)
            {
                return false;
            }

            return IsOk(response.Status);
        }

        public static bool IsSuccess(ResponseOfGetLiveCommentorResponse response)
        {
            if (response == null)
            {
                return false;
            }

            return IsOk(response.Status);
        }

        public static bool IsSuccess(ResponseOfUnlockBookingResponse response)
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