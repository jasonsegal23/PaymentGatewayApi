using PaymentGateway.Commands;
using PaymentGateway.Responses;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public interface IBankService
    {
        public Task<PaymentResponse> ProcessPayment(MakePaymentCommand request);
    }
}
