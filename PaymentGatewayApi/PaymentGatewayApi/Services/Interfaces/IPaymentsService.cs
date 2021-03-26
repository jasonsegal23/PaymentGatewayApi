using PaymentGateway.Models.Payment;
using System.Threading.Tasks;

namespace PaymentGateway.Services.Interfaces
{
    public interface IPaymentsService
    {
        public Task<Payment> CreateAsync(Payment payment);

        public Task<Payment> GetAsync(string id);
    }
}
