using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Common.Constants;
using SC.App.Services.Bill.Configurations.Middlewares;

namespace SC.App.Services.Bill.Configurations.Extensions
{
    public static class LocalizeExtension
    {
        public static void AddLocalize(this IServiceCollection services, IConfiguration configuration)
        {
            var defaultCulture = configuration.GetValue<string>(AppSettings.Culture.Default);
            var supportedCultures = configuration.GetSection(AppSettings.Culture.Supports).Get<string[]>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture(defaultCulture);
                options.AddSupportedCultures(supportedCultures);
                options.FallBackToParentCultures = true;
            });
        }

        public static void UseLocalize(this IApplicationBuilder app)
        {
            app.UseMiddleware<LocalizeMiddleware>();
        }
    }
}