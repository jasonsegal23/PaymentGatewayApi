using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PaymentGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(o =>
                    {
                        o.ListenAnyIP(
                            5001,
                            listenOptions => listenOptions.UseHttps("payment-gateway.pfx"));
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
