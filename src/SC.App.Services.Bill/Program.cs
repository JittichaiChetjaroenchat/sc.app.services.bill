using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SC.App.Services.Bill.Configurations.Extensions;

namespace SC.App.Services.Bill
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}