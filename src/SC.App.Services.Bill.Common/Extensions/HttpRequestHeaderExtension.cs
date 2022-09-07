using System.Net;
using System.Text;

namespace SC.App.Services.Bill.Common.Extensions
{
    public static class HttpRequestHeaderExtension
    {
        public static string ToStandardName(this HttpRequestHeader requestHeader)
        {
            string headerName = requestHeader.ToString();

            var headerStandardNameBuilder = new StringBuilder();
            headerStandardNameBuilder.Append(headerName[0]);

            for (int index = 1; index < headerName.Length; index++)
            {
                char character = headerName[index];
                if (char.IsUpper(character))
                {
                    headerStandardNameBuilder.Append('-');
                }

                headerStandardNameBuilder.Append(character);
            }

            string headerStandardName = headerStandardNameBuilder.ToString();

            return headerStandardName;
        }
    }
}