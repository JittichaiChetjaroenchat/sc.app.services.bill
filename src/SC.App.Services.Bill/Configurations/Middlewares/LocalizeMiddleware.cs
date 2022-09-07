using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SC.App.Services.Bill.Business.Resources;
using SC.App.Services.Bill.Common.Constants;
using SC.App.Services.Bill.Common.Extensions;
using SC.App.Services.Bill.Lib.Extensions;

namespace SC.App.Services.Bill.Configurations.Middlewares
{
    public class LocalizeMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly IConfiguration _configuration;

        public LocalizeMiddleware(
            RequestDelegate requestDelegate,
            IConfiguration configuration)
        {
            _requestDelegate = requestDelegate;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var acceptLanguage = context.Request.GetAcceptLanguage();
            if (!acceptLanguage.IsEmpty())
            {
                var defaultCulture = _configuration.GetValue<string>(AppSettings.Culture.Default);
                var supportedCultures = _configuration.GetSection(AppSettings.Culture.Supports).Get<string[]>();
                var culture = supportedCultures.Contains(acceptLanguage) ? acceptLanguage : defaultCulture;

                try
                {
                    var cultureInfo = CultureInfo.GetCultureInfo(culture);
                    ErrorMessage.Culture = cultureInfo;
                }
                catch { }
            }

            await _requestDelegate.Invoke(context);
        }
    }
}