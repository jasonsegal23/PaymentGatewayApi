using PaymentGateway.Models.Billing;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    public class BillingDetails
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string CardType { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public int ExpiryMonth { get; set; }

        [Required]
        public int ExpiryYear { get; set; }

        [Required]
        public string Cvv { get; set; }

        [Required]
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
