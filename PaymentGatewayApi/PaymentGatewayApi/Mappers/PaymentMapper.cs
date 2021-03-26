using PaymentGateway.Mappers.Interfaces;
using PaymentGateway.Models.Payment;
using PaymentGateway.Responses;

namespace PaymentGateway
{
    public class PaymentMapper : IPaymentMapper
    {
        public PaymentResponse Map(Payment payment)
        {
            var paymentResponse = new PaymentResponse
            {
                Id = payment.Id,
                BankPaymentId = payment.BankPaymentId,
                MerchantPaymentId = payment.MerchantPaymentId,
                BillingDetails = payment.BillingDetails,
                Currency = payment.Currency,
                Amount = payment.Amount,
                Status = payment.Status,
                ValidationErrors = payment.ValidationErrors,
                Reason = payment.Reason,
                TimeStamp = payment.Timestamp
            };

            // Mask card details
            var maskedCardNumber = payment.BillingDetails.MaskCardNumber();
            paymentResponse.BillingDetails.CardNumber = maskedCardNumber;

            return paymentResponse;
        }
    }
}
