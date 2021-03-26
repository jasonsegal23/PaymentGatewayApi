using PaymentGateway.Commands;
using PaymentGateway.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentGateway.Services
{
    public class FakeBankValidatorService : IFakeBankValidatorService
    {
        public (bool, List<ValidationError>) Validate(MakePaymentCommand request)
        {
            var validationErrors = new List<ValidationError>();

            // Fake validators for the bank
            for(int i = 0; i < 5; i++)
            {
                var validatorErrors = FakeValidatior(request);

                if(validationErrors.Any())
                {
                    validationErrors.AddRange(validatorErrors);
                }
            }

            var isValid = !validationErrors.Any();

            return (isValid, validationErrors);
        }

        public IEnumerable<ValidationError> FakeValidatior(MakePaymentCommand request)
        {
            // Create list for validation errors
            var list = new List<ValidationError>();

            // Pick 50/50 probability of getting an error
            var prob = new Random().Next(50) <= 25;

            // Add validation errors based on probability result
            if (prob)
            {
                list.Add(new ValidationError("Fake validation error"));
            }

            return list;
        }
    }
}
