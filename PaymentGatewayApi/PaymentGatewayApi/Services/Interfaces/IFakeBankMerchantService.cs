using PaymentGateway.Commands;

namespace PaymentGateway.Services.Interfaces
{
    public interface IFakeBankMerchantService
    {
        public bool ProcessFundsTransfer(MakePaymentCommand request);
    }
}
