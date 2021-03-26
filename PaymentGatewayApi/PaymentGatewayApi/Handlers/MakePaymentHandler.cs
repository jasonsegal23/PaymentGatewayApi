using MediatR;
using PaymentGateway.Commands;
using PaymentGateway.Mappers.Interfaces;
using PaymentGateway.Models.Payment;
using PaymentGateway.Responses;
using PaymentGateway.Services;
using PaymentGateway.Services.Interfaces;
using PaymentGateway.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace PaymentGateway.Handlers
{
    public class MakePaymentHandler : IRequestHandler<MakePaymentCommand, PaymentResponse>
    {
        private readonly IBankService _bankService;
        private readonly IPaymentsService _paymentsService;
        private readonly IValidatorService _validatorService;
        private readonly IEnumerable<IPaymentValidator> _validators;
        private readonly IPaymentMapper _paymentMapper;

        public MakePaymentHandler(
            IBankService bankService, 
            IPaymentsService paymentsService, 
            IValidatorService validatorService, 
            IEnumerable<IPaymentValidator> validators, 
            IPaymentMapper paymentMapper)
        {
            _bankService = bankService;
            _paymentsService = paymentsService;
            _validatorService = validatorService;
            _validators = validators;
            _paymentMapper = paymentMapper;
        }

        public async Task<PaymentResponse> Handle(MakePaymentCommand request, CancellationToken cancellationToken)
        {
            // Create ID for internal MakePayment event
            var id = Guid.NewGuid().ToString();

            // Server side validations
            var (isValid, validationErrors) = _validatorService.Validate(_validators, request);

            if (!isValid)
            {
                return new PaymentResponse
                {
                    Id = id,
                    Status = "failure",
                    ValidationErrors = validationErrors,
                    Reason = "Failed to validate payment details"
                };
            }

            // Forward payment request
            var response = await _bankService.ProcessPayment(request);

            if(response == null)
            {
                return new PaymentResponse
                {
                    Id = id,
                    Status = "failure",
                    ValidationErrors = validationErrors,
                    Reason = "Failed to process payment details"
                };
            }

            var payment = new Payment()
            {
                Id = id,
                BankPaymentId = response.BankPaymentId,
                MerchantPaymentId = request.MerchantPaymentId,
                BillingDetails = request.BillingDetails,
                Currency = request.Currency,
                Amount = request.Amount,
                Status = response.Status,
                Reason = response.Reason,
                ValidationErrors = response.ValidationErrors,
                Timestamp = DateTime.UtcNow,
            };

            // Save payment event to payments DB (failure or success)
            await _paymentsService.CreateAsync(payment);

            // Map from payment to PaymentResponse object
            var paymentResponse = _paymentMapper.Map(payment);

            return paymentResponse;
        }
    }
}
