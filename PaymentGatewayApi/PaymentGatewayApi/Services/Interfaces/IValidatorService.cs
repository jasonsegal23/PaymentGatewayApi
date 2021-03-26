using PaymentGateway.Commands;
using PaymentGateway.Validators;
using PaymentGateway.Validators.Interfaces;
using System.Collections.Generic;

namespace PaymentGateway.Services.Interfaces
{
    public interface IValidatorService
    {
        public (bool, List<ValidationError>) Validate(IEnumerable<IPaymentValidator> paymentValidators, MakePaymentCommand request);
    }
}
