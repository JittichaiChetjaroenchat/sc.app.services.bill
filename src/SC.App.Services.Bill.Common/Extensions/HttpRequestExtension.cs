using System.Net;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Common.Extensions
{
    public static class HttpRequestExtension
    {
        public const string Scheme = "Bearer";

        public static void CreateAuthorization(this HttpRequest request, string value)
        {
            if (request != null)
            {
                request.Headers.Add($"{HttpRequestHeader.Authorization}", $"{Scheme} {value}");
                request.Headers.Add($"X-{HttpRequestHeader.Authorization}", $"{Scheme} {value}");
            }
        }

        public static string GetAuthorization(this HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }

            string token = null;

            // Get from standard header
            token = request.Headers[HttpRequestHeader.Authorization.ToString()].ToString();
            if (!token.IsEmpty())
            {
                return token;
            }

            // Get from custom header
            token = request.Headers[$"X-{HttpRequestHeader.Authorization}"].ToString();
            if (!token.IsEmpty())
            {
                return token;
            }

            return null;
        }

        public static string GetAcceptLanguage(this HttpRequest request)
        {
            if (request == null)
            {
                return null;
            }

            string language = null;

            // Get from standard header
            language = request.Headers[HttpRequestHeader.AcceptLanguage.ToStandardName()].ToString();
            if (!language.IsEmpty())
            {
                return language;
            }

            // Get from custom header
            language = request.Headers[$"X-{HttpRequestHeader.AcceptLanguage.ToStandardName()}"].ToString();
            if (!language.IsEmpty())
            {
                return language;
            }

            return null;
        }

        public static string GetAccessToken(this HttpRequest request)
        {
            var authorization = request.GetAuthorization();
            if (authorization.IsEmpty())
            {
                return null;
            }

            return authorization
                .Replace(Scheme, string.Empty)
                .Trim();
        }
    }
}