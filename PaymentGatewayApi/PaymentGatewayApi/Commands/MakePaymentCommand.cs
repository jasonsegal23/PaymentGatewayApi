using MediatR;
using PaymentGateway.Models;
using PaymentGateway.Responses;

namespace PaymentGateway.Commands
{
    public class MakePaymentCommand : IRequest<PaymentResponse>
    {
        public string MerchantPaymentId { get; set; }

        public BillingDetails BillingDetails { get; set; }

        public string Currency { get; set; }

        public double Amount { get; set; }
    }
}
