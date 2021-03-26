using PaymentGateway.Models;
using PaymentGateway.Validators;
using System;
using System.Collections.Generic;

namespace PaymentGateway.Responses
{
    public class PaymentResponse
    {
        public string Id { get; set; }

        public string BankPaymentId { get; set; }

        public string MerchantPaymentId { get; set; }

        public BillingDetails BillingDetails { get; set; }

        public string Currency { get; set; }

        public double Amount { get; set; }

        public string Status { get; set; }

        public List<ValidationError> ValidationErrors { get; set; }

        public string Reason { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
