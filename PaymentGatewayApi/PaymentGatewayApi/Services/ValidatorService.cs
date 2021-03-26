using PaymentGateway.Commands;
using PaymentGateway.Exceptions;
using PaymentGateway.Services.Interfaces;
using PaymentGateway.Validators;
using PaymentGateway.Validators.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PaymentGateway.Services
{
    public class ValidatorService : IValidatorService
    {
        public (bool, List<ValidationError>) Validate(IEnumerable<IPaymentValidator> paymentValidators, MakePaymentCommand request)
        {
            if (paymentValidators.Any())
            {
                var responses = paymentValidators.Select(v => v.Validate(request));
                var isValid = responses.All(a => !a.Any());
                var validationErrors = !isValid ? responses.SelectMany(r => r.Where(a => a != null)).ToList() : null;
                return (isValid, validationErrors);
            }

            throw new PaymentValidatorsNotImplementedException("Payment validators must be implemented");
        }
    }
}
