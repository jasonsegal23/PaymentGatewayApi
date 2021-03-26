using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Compact;

namespace PaymentGateway.infrastructure
{
    public static class LoggingServices
    {
        public static void AddLoggingServices(this IServiceCollection services, IConfiguration configs)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(new CompactJsonFormatter())
                .CreateLogger();

            services.AddSingleton(Log.Logger);
        }
    }
}
