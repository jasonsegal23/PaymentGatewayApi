using Lucene.Net.Support;
using PaymentGateway.Commands;
using PaymentGateway.Validators.Interfaces;
using System.Collections.Generic;

namespace PaymentGateway.Validators
{
    public class CardNumberLengthValidator : IPaymentValidator
    {
        private readonly string ValidationMessage = "Validation Failure - Card Number Length Incorrect";

        public IEnumerable<ValidationError> Validate(MakePaymentCommand request)
        {
            var list = new List<ValidationError>();
            var cardNumber = request.BillingDetails.CardNumber.Replace(" ", string.Empty);

            var allowedLengths = new HashMap<string, List<int>>
            {
                { "visa", new List<int> { 13, 16, 19 } },
                { "amex", new List<int> { 15 } },
                { "mastercard", new List<int> { 16 } }
            };

            switch (request.BillingDetails.CardType)
            {
                case "visa":
                    allowedLengths.TryGetValue("visa", out var allowedLengthList);
                    if (!allowedLengthList.Contains(cardNumber.Length)) { list.Add(new ValidationError(ValidationMessage)); };
                    break;
                case "amex":
                    allowedLengths.TryGetValue("amex", out allowedLengthList);
                    if (!allowedLengthList.Contains(cardNumber.Length)) { list.Add(new ValidationError(ValidationMessage)); };
                    break;
                case "mastercard":
                    allowedLengths.TryGetValue("mastercard", out allowedLengthList);
                    if (!allowedLengthList.Contains(cardNumber.Length)) { list.Add(new ValidationError(ValidationMessage)); };
                    break;
                default:
                    break;
            }

            return list;
        }
    }
}
