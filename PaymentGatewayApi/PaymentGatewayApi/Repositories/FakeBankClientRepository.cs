using PaymentGateway.Commands;
using PaymentGateway.Repositories.Interfaces;
using System;

namespace PaymentGateway.Repositories
{
    public class FakeBankClientRepository: IFakeBankClientRepository
    {
        public bool RequestFunds(MakePaymentCommand request)
        {
            var random = new Random();
            return random.Next(100) <= 50;
        }
    }
}
