using Microsoft.AspNetCore.Http;

namespace SC.App.Services.Bill.Lib.Helpers
{
    public static class UriHelper
    {
        public static string BuildLocation(HttpRequest request, object value)
        {
            return $"{request.Scheme}://{request.Host.Value}{request.Path}/{value}";
        }
    }
}