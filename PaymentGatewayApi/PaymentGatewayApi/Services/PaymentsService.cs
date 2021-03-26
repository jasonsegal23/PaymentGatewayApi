using MongoDB.Driver;
using PaymentGateway.Configuration;
using PaymentGateway.Models.Payment;
using PaymentGateway.Services.Interfaces;
using System.Threading.Tasks;

namespace PaymentGateway.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IMongoCollection<Payment> _payments;

        public PaymentsService(IPaymentsDatabaseConfig settings)
        {
            var client = new MongoClient(settings.ConnectionString);

            var database = client.GetDatabase(settings.DatabaseName);

            _payments = database.GetCollection<Payment>(settings.PaymentsCollectionName);
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            await _payments.InsertOneAsync(payment);
            return await Task.FromResult(payment);
        }

        public async Task<Payment> GetAsync(string id)
        {
            var payment = await _payments.FindAsync(payment => payment.Id == id);
            return await Task.FromResult(payment.FirstOrDefault());
        }
    }
}
