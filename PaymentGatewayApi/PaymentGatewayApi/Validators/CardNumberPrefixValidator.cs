using Lucene.Net.Support;
using PaymentGateway.Commands;
using PaymentGateway.Validators.Interfaces;
using System.Collections.Generic;

namespace PaymentGateway.Validators
{
    public class CardNumberPrefixValidator : IPaymentValidator
    {
        public IEnumerable<ValidationError> Validate(MakePaymentCommand request)
        {
            var list = new List<ValidationError>();
            var cardNumber = request.BillingDetails.CardNumber.Replace(" ", string.Empty);

            var allowedPrefixes = new HashMap<string, List<int>>
            {
                { "visa", new List<int> { 4 } },
                { "amex", new List<int> { 34, 37 } },
                { "mastercard", new List<int> { 51, 52, 53, 54, 55 } }
            };

            int prefixDigits;
            switch (request.BillingDetails.CardType)
            {
                case "visa":
                    prefixDigits = int.Parse(cardNumber.Substring(0, 1));
                    allowedPrefixes.TryGetValue("visa", out var allowedValues);
                    if (!allowedValues.Contains(prefixDigits)) { list.Add(new ValidationError("Validation Failure - Visa Card Number Prefix Incorrect")); };
                    break;
                case "amex":
                    prefixDigits = int.Parse(cardNumber.Substring(0, 2));
                    allowedPrefixes.TryGetValue("amex", out allowedValues);
                    if (!allowedValues.Contains(prefixDigits)) { list.Add(new ValidationError("Validation Failure - Amex Card Number Prefix Incorrect")); };
                    break;
                case "mastercard":
                    prefixDigits = int.Parse(cardNumber.Substring(0, 2));
                    allowedPrefixes.TryGetValue("mastercard", out allowedValues);
                    if (!allowedValues.Contains(prefixDigits)) { list.Add(new ValidationError("Validation Failure - Mastercard Card Number Prefix Incorrect")); }
                    break;
                default:
                    break;

            }

            return list;
        }
    }
}
