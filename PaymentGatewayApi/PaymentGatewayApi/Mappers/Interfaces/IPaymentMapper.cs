using PaymentGateway.Models.Payment;
using PaymentGateway.Responses;

namespace PaymentGateway.Mappers.Interfaces
{
    public interface IPaymentMapper
    {
        public PaymentResponse Map(Payment payment);
    }
}
