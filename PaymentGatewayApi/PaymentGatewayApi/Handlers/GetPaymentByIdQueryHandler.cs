using MediatR;
using PaymentGateway.Mappers.Interfaces;
using PaymentGateway.Queries;
using PaymentGateway.Responses;
using PaymentGateway.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Handlers
{
    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentResponse>
    {
        private readonly IPaymentsService _paymentsService;
        private readonly IPaymentMapper _paymentMapper;

        public GetPaymentByIdQueryHandler(IPaymentsService paymentsService, IPaymentMapper paymentMapper)
        {
            _paymentsService = paymentsService;
            _paymentMapper = paymentMapper;
        }

        public async Task<PaymentResponse> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            // Retrieve payment from DB by Id
            var payment = await _paymentsService.GetAsync(request.Id);

            if (payment == null)
            {
                return new PaymentResponse
                {
                    Id = request.Id,
                    Status = "failure",
                    Reason = "Failed to fetch payment"
                };
            }

            payment.Status = "success";
            payment.Reason = "Payment fetched successfully";

            // Map Payment to PaymentResponse object
            var paymentResponse = _paymentMapper.Map(payment);

            return paymentResponse;
        }
    }
}
