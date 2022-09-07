using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SC.App.Services.Bill.Common.Extensions;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Client
{
    public abstract class BaseHttpClient : IBaseHttpClient
    {
        public string Authorization { get; private set; }

        public string AcceptLanguage { get; private set; }

        public void SetAuthorization(string authorization)
        {
            Authorization = authorization;
        }

        public void SetAcceptLanguage(string acceptLanguage)
        {
            AcceptLanguage = acceptLanguage;
        }

        // Called by implementing swagger client classes
        protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage();

            // Set authorization
            if (!Authorization.IsEmpty())
            {
                var authorizations = Authorization.Split(" ");

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(authorizations[0], authorizations[1]);
                request.Headers.Add($"X-{HttpRequestHeader.Authorization}", Authorization);
            }

            // Set accept-language
            if (!AcceptLanguage.IsEmpty())
            {
                request.Headers.Add(HttpRequestHeader.AcceptLanguage.ToStandardName(), AcceptLanguage);
                request.Headers.Add($"X-{HttpRequestHeader.AcceptLanguage.ToStandardName()}", AcceptLanguage);
            }

            return Task.FromResult(request);
        }
    }
}