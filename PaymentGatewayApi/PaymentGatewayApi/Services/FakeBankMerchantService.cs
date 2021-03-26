using PaymentGateway.Commands;
using PaymentGateway.Services.Interfaces;

namespace PaymentGateway.Services
{
    public class FakeBankMerchantService : IFakeBankMerchantService
    {
        public bool ProcessFundsTransfer(MakePaymentCommand request)
        {
            // TODO - Need to fake repository request for processing funds to merchant
            return true;
        }
    }
}
