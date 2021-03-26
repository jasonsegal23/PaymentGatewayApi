using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PaymentGateway.Validators;
using System;
using System.Collections.Generic;

namespace PaymentGateway.Models.Payment
{
    [Serializable]
    public class Payment
    {
        [BsonId]
        public string Id { get; set; }

        public string BankPaymentId { get; set; }

        public string MerchantPaymentId { get; set; }

        public BillingDetails BillingDetails { get; set; }

        public string Currency { get; set; }

        public double Amount { get; set; }

        public string Status { get; set; }

        public List<ValidationError> ValidationErrors;

        public string Reason;

        public DateTime Timestamp;
    }
}
