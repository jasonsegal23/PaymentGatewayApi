using MediatR;
using PaymentGateway.Responses;

namespace PaymentGateway.Queries
{
    public class GetPaymentByIdQuery: IRequest<PaymentResponse>
    {
        public string Id { get; set; }

        public GetPaymentByIdQuery(string id)
        {
            Id = id;
        }
    }
}
