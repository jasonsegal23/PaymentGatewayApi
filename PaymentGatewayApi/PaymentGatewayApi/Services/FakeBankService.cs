using PaymentGateway.Commands;
using PaymentGateway.Responses;
using PaymentGateway.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public class FakeBankService : IBankService
    {
        public readonly IFakeBankValidatorService _fakeBankValidatorService;
        public readonly IFakeBankClientService _fakeBankClientService;
        public readonly IFakeBankMerchantService _fakeBankMerchantService;

        public FakeBankService(
            IFakeBankValidatorService fakeBankValidatorService,
            IFakeBankClientService fakeBankClientService,
            IFakeBankMerchantService fakeBankMerchantService)
        {
            _fakeBankValidatorService = fakeBankValidatorService;
            _fakeBankClientService = fakeBankClientService;
            _fakeBankMerchantService = fakeBankMerchantService;
        }

        public Task<PaymentResponse> ProcessPayment(MakePaymentCommand request)
        {
            // Set Bank Payment ID for event
            var id = new Guid().ToString();

            // Bank Validations
            var (isValid, validationErrors) = _fakeBankValidatorService.Validate(request);

            if (!isValid)
            {
                return Task.FromResult(new PaymentResponse
                {
                    BankPaymentId = id,
                    Status = "failure",
                    ValidationErrors = validationErrors,
                    Reason = "Failed to validate payment details"
                });
            }

            // Client Funds Verification
            isValid = _fakeBankClientService.RequestFunds(request);

            if (!isValid)
            {
                return Task.FromResult(new PaymentResponse
                {
                    BankPaymentId = id,
                    Status = "failure",
                    Reason = "Failed to verify client funds"
                });
            }

            // Merchant Money Processing
            isValid = _fakeBankMerchantService.ProcessFundsTransfer(request);

            if (!isValid)
            {
                return Task.FromResult(new PaymentResponse
                {
                    BankPaymentId = id,
                    Status = "failure",
                    Reason = "Failed to process funds with merchant"
                });
            }

            return Task.FromResult(new PaymentResponse
            {
                BankPaymentId = id,
                Status = "success"
            });

        }
    }
}
