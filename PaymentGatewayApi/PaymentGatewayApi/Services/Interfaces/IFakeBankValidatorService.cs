using PaymentGateway.Commands;
using PaymentGateway.Validators;
using PaymentGateway.Validators.Interfaces;
using System.Collections.Generic;

namespace PaymentGateway.Services
{
    public interface IFakeBankValidatorService
    {
        public (bool, List<ValidationError>) Validate(MakePaymentCommand request);
    }
}
