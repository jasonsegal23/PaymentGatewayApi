using PaymentGateway.Models;
using System;

namespace PaymentGateway.Dtos
{
    public class MakePaymentCommandDto
    {
        public string MerchantPaymentId { get; set; }

        public BillingDetails BillingDetails { get; set; }

        public string Currency { get; set; }

        public double Amount { get; set; }
    }
}
