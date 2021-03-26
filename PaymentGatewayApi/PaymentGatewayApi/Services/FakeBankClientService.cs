using PaymentGateway.Commands;
using PaymentGateway.Repositories.Interfaces;
using PaymentGateway.Services.Interfaces;

namespace PaymentGateway.Services
{
    public class FakeBankClientService: IFakeBankClientService
    {
        IFakeBankClientRepository _fakeBankClientRepository;

        public FakeBankClientService(IFakeBankClientRepository fakeBankClientRepository)
        {
            _fakeBankClientRepository = fakeBankClientRepository;
        }

        public bool RequestFunds(MakePaymentCommand request)
        {
            return _fakeBankClientRepository.RequestFunds(request);
        }

    }
}
