using PaymentGateway.Commands;

namespace PaymentGateway.Repositories.Interfaces
{
    public interface IFakeBankClientRepository
    {
        public bool RequestFunds(MakePaymentCommand request);
    }
}
