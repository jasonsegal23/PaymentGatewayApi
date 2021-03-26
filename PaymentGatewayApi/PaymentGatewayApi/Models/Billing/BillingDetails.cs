using PaymentGateway.Models.Billing;

namespace PaymentGateway.Models
{
    public class BillingDetails
    {
        public string Name { get; set; }

        public string CardType { get; set; }

        public string CardNumber { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public string Cvv { get; set; }

        public BillingAddress BillingAddress { get; set; }

        public string MaskCardNumber()
        {
            var cardNumber = CardNumber;
            var asterixes = new string('*', cardNumber.Length - 4);
            var lastFourDigits = cardNumber.Substring(cardNumber.Length - 4);
            return $"{asterixes}{lastFourDigits}";
        }
    }
}
