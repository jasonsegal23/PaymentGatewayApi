using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PaymentGateway.Configuration;

namespace PaymentGateway.infrastructure
{
    public static class DatabaseServices
    {
        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configs)
        {
            services.Configure<PaymentsDatabaseConfig>(
                configs.GetSection(nameof(PaymentsDatabaseConfig)));

            services.AddSingleton<IPaymentsDatabaseConfig>(sp =>
                sp.GetRequiredService<IOptions<PaymentsDatabaseConfig>>().Value);
        }
    }
}
