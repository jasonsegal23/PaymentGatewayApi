using System;

namespace PaymentGateway.Exceptions
{
    public class PaymentValidatorsNotImplementedException : Exception
    {
        private readonly string _error;

        public PaymentValidatorsNotImplementedException(string error)
        {
            _error = error;
        }
    }
}
