using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SC.App.Services.Bill.Common.Constants;
using SC.App.Services.Bill.Database;

namespace SC.App.Services.Bill.Configurations.Extensions
{
    public static class DatabaseExtension
    {
        public static void AddDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>(AppSettings.Databases.Bill.ConnectionString);

            // Add database context
            var version = ServerVersion.AutoDetect(connectionString);
            services.AddDbContextPool<DatabaseContext>(options =>
            {
                options.UseMySql(connectionString, version)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
            });
        }

        public static void UseDatabases(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();

            context.Database.Migrate();
        }
    }
}