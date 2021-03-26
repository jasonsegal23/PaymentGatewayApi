using AutoMapper;
using PaymentGateway.Commands;
using PaymentGateway.Dtos;

namespace PaymentGateway.Mappings
{
    public class PaymentProfile: Profile
    {
        public PaymentProfile()
        {
            CreateMap<MakePaymentCommandDto, MakePaymentCommand>();
            CreateMap<MakePaymentCommand, MakePaymentCommandDto>();
        }
    }
}
