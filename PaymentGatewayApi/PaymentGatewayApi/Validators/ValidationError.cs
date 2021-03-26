using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Validators
{
    public class ValidationError
    {
        private readonly string _message;

        public ValidationError(string message)
        {
            _message = message;
        }
    }
}
