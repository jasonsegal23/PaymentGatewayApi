using PaymentGateway.Commands;

namespace PaymentGateway.Services.Interfaces
{
    public interface IFakeBankClientService
    {
        public bool RequestFunds(MakePaymentCommand request);
    }
}
