using PaymentGateway.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentGateway.Validators.Interfaces
{
    public interface IPaymentValidator
    {
        public IEnumerable<ValidationError> Validate(MakePaymentCommand request);
    }
}
