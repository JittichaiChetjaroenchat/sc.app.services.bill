using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NSwag;

namespace SC.App.Services.Bill.Configurations.Extensions
{
    public static class DocumentExtension
    {
        public static void AddDocument(this IServiceCollection services, string title)
        {
            services.AddSwaggerDocument(options =>
            {
                options.AllowReferencesWithProperties = true;
                options.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = title;

                    document.Schemes.Clear();
                    document.Schemes.Add(OpenApiSchema.Http);
                    document.Schemes.Add(OpenApiSchema.Https);
                };
            });
        }

        public static void UseDocument(this IApplicationBuilder app)
        {
            app.UseOpenApi(options =>
            {
                options.PostProcess = (document, _) =>
                {
                    document.Schemes = new[] { OpenApiSchema.Http, OpenApiSchema.Https };
                };
            });
            app.UseSwaggerUi3(options =>
            {
                options.OperationsSorter = "alpha";
                options.TagsSorter = "alpha";
            });
        }
    }
}