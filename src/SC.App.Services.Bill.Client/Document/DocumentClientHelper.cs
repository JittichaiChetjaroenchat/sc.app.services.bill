using System.Collections.Generic;
using System.Linq;
using SC.App.Services.Document.Client;

namespace SC.App.Services.Bill.Client.Document
{
    public class DocumentClientHelper
    {
        public static bool IsSuccess(ResponseOfGetDocumentResponse response)
        {
            if (response == null)
            {
                return false;
            }

            return IsOk(response.Status);
        }

        public static bool IsSuccess(ResponseOfListOfGetDocumentResponse response)
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