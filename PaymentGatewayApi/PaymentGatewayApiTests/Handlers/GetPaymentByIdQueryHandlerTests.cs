using FakeItEasy;
using NUnit.Framework;
using PaymentGateway.Handlers;
using PaymentGateway.Mappers.Interfaces;
using PaymentGateway.Models;
using PaymentGateway.Models.Billing;
using PaymentGateway.Models.Payment;
using PaymentGateway.Queries;
using PaymentGateway.Responses;
using PaymentGateway.Services.Interfaces;
using PaymentGateway.Validators;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGatewayApiTests.Handlers
{
    [TestFixture]
    class GetPaymentByIdQueryHandlerTests
    {
        public GetPaymentByIdQueryHandler _sut;

        public IPaymentsService _paymentsService = A.Fake<IPaymentsService>();
        public IPaymentMapper _paymentMapper = A.Fake<IPaymentMapper>();

        [SetUp]
        public void SetUp()
        {
            _sut = new GetPaymentByIdQueryHandler(_paymentsService, _paymentMapper);
        }

        [Test]
        public async Task HandleGetPaymentByIdQuery_ReturnsSuccessfulPaymentResponse_WhenPaymentFoundSuccessfully()
        {
            // Arrange
            var id = new Guid().ToString();
            var request = new GetPaymentByIdQuery(new Guid().ToString());
            var payment = CreateFakePayment(id);

            var paymentResponse = CreateSuccessfulPaymentResponse(id);

            A.CallTo(() => _paymentsService.GetAsync(request.Id)).Returns(payment);
            A.CallTo(() => _paymentMapper.Map(payment)).Returns(paymentResponse);


            // Act
            var response = await _sut.Handle(request, new CancellationToken());

            // Assert
            Assert.AreEqual(paymentResponse, response);

        }

        [Test]
        public async Task HandleGetPaymentByIdQuery_ReturnsFailurePaymentResponse_WhenPaymentNotFound()
        {
            // Arrange
            var id = new Guid().ToString();
            var request = new GetPaymentByIdQuery(new Guid().ToString());

            var paymentResponse = CreateFailurePaymentResponse(id);

            A.CallTo(() => _paymentsService.GetAsync(request.Id)).Returns<Payment>(null);

            // Act
            var response = await _sut.Handle(request, new CancellationToken());

            // Assert
            Assert.AreEqual(paymentResponse.Id, response.Id);
            Assert.AreEqual(paymentResponse.Status, response.Status);
            Assert.AreEqual(paymentResponse.Reason, response.Reason);
        }

        private static Payment CreateFakePayment(string id)
        {
            return new Payment
            {
                Id = id,
                BankPaymentId = Guid.NewGuid().ToString(),
                MerchantPaymentId = Guid.NewGuid().ToString(),
                BillingDetails = new BillingDetails
                {
                    Name = "Test User",
                    CardType = "visa",
                    CardNumber = "4000100020003000",
                    ExpiryMonth = 12,
                    ExpiryYear = 2022,
                    Cvv = "155",
                    BillingAddress = new BillingAddress()
                    {
                        Title = "Mr",
                        FirstName = "Test",
                        LastName = "User",
                        Country = "United Kingdom",
                        Address1 = "Test Address Line 1",
                        Address2 = "Test Address Line 2",
                        Postcode = "ABC DEF"
                    }
                },
                Currency = "GBP",
                Amount = 100,
                Status = "success",
                ValidationErrors = new List<ValidationError>(),
                Reason = "",
                Timestamp = DateTime.UtcNow
            };
        }
        
        private static PaymentResponse CreateSuccessfulPaymentResponse(string id)
        {
            return new PaymentResponse
            {
                Id = id,
                BankPaymentId = Guid.NewGuid().ToString(),
                MerchantPaymentId = Guid.NewGuid().ToString(),
                BillingDetails = new BillingDetails
                {
                    Name = "Test User",
                    CardType = "visa",
                    CardNumber = "4000100020003000",
                    ExpiryMonth = 12,
                    ExpiryYear = 2022,
                    Cvv = "155",
                    BillingAddress = new BillingAddress()
                    {
                        Title = "Mr",
                        FirstName = "Test",
                        LastName = "User",
                        Country = "United Kingdom",
                        Address1 = "Test Address Line 1",
                        Address2 = "Test Address Line 2",
                        Postcode = "ABC DEF"
                    }
                },
                Currency = "GBP",
                Amount = 100,
                Status = "success",
                ValidationErrors = new List<ValidationError>(),
                Reason = "Payment fetched successfully"
            };
        }

        private PaymentResponse CreateFailurePaymentResponse(string id)
        {
            return new PaymentResponse
            {
                Id = id,
                Status = "failure",
                Reason = "Failed to fetch payment"
            };
        }
    }
}
