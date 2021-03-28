using PaymentGateway.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Dtos
{
    public class MakePaymentCommandDto
    {
        [Required]
        public string MerchantPaymentId { get; set; }

        [Required]
        public BillingDetails BillingDetails { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public double Amount { get; set; }
    }
}
