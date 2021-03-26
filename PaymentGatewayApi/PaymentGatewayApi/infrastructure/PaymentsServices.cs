using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Commands;
using PaymentGateway.Configuration;
using PaymentGateway.Dtos;
using PaymentGateway.Mappers.Interfaces;
using PaymentGateway.Mappings;
using PaymentGateway.Repositories;
using PaymentGateway.Repositories.Interfaces;
using PaymentGateway.Services.Interfaces;
using PaymentGateway.Validators;
using PaymentGateway.Validators.Interfaces;

namespace PaymentGateway.Services
{
    public static class PaymentsServices
    {
        public static void AddPaymentServices(this IServiceCollection services, IConfiguration config)
        {
            // Add bank services
            var bankConfig = new BankConfig();
            config.GetSection(nameof(BankConfig)).Bind(bankConfig);
            if (bankConfig.UseFake) 
            {
                services.AddTransient<IBankService, FakeBankService>();
                services.AddTransient<IFakeBankValidatorService, FakeBankValidatorService>();
                services.AddTransient<IFakeBankClientService, FakeBankClientService>();
                services.AddTransient<IFakeBankClientRepository, FakeBankClientRepository>();
                services.AddTransient<IFakeBankMerchantService, FakeBankMerchantService>();
            }
            
            // Add validation services

            // Add payment services
            services.AddSingleton<IPaymentsService, PaymentsService>();
            services.AddSingleton<IPaymentMapper, PaymentMapper>();
            services.AddSingleton<IPaymentValidator, CardNumberPrefixValidator>();
            services.AddSingleton<IValidatorService, ValidatorService>();

            // Add automapper configs
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.CreateMap<MakePaymentCommandDto, MakePaymentCommand>();
                mc.AddProfile<PaymentProfile>();
            });
        }

    }
}
