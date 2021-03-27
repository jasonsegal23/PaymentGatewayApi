using FakeItEasy;
using NUnit.Framework;
using PaymentGateway.Commands;
using PaymentGateway.Handlers;
using PaymentGateway.Mappers.Interfaces;
using PaymentGateway.Models;
using PaymentGateway.Models.Billing;
using PaymentGateway.Models.Payment;
using PaymentGateway.Responses;
using PaymentGateway.Services;
using PaymentGateway.Services.Interfaces;
using PaymentGateway.Validators;
using PaymentGateway.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using System.Threading.Tasks;

namespace PaymentGatewayApiTests.Handlers
{
    [TestFixture]
    class MakePaymentHandlerTests
    {
        public MakePaymentHandler _sut;

        private readonly IBankService _bankService = A.Fake<IBankService>();
        private readonly IPaymentsService _paymentsService = A.Fake<IPaymentsService>();
        private readonly IValidatorService _validatorService = A.Fake<IValidatorService>();
        private readonly IEnumerable<IPaymentValidator> _validators = A.Fake<IEnumerable<IPaymentValidator>>();
        private readonly IEnumerator<IPaymentValidator> _validatorsEnumerator = A.Fake<IEnumerator<IPaymentValidator>>();
        private readonly IPaymentMapper _paymentMapper = A.Fake<IPaymentMapper>();

        [SetUp]
        public void SetUp()
        {
            _sut = new MakePaymentHandler(_bankService, _paymentsService, 
                _validatorService, _validators, _paymentMapper);
        }

        [Test]
        public async Task HandlePayment_ReturnSuccessfulPaymentResponse_WhenPaymentPassedAllChecks()
        {
            // Arrange
            var request = CreateMakePaymentCommand();
            var (payment, paymentResponse) = CreateSuccessfulPaymentAndPaymentResponse();

            A.CallTo(() => _validatorsEnumerator.Current).Returns(new CardNumberLengthValidator());
            A.CallTo(() => _validators.GetEnumerator()).Returns(_validatorsEnumerator);
            A.CallTo(() => _validatorService.Validate(_validators, request)).Returns((true, new List<ValidationError>()));
            A.CallTo(() => _bankService.ProcessPayment(request)).Returns(paymentResponse);
            A.CallTo(() => _paymentsService.CreateAsync(payment));
            A.CallTo(() => _paymentMapper.Map(payment)).Returns(paymentResponse);

            // Act
            var response = await _sut.Handle(request, new CancellationToken());

            // Assert
            var expectedPaymentResponse = new PaymentResponse
            {
                Id = paymentResponse.Id,
                BankPaymentId = paymentResponse.BankPaymentId,
                MerchantPaymentId = paymentResponse.MerchantPaymentId,
                BillingDetails = paymentResponse.BillingDetails,
                Currency = paymentResponse.Currency,
                Amount = paymentResponse.Amount,
                Status = paymentResponse.Status,
                ValidationErrors = paymentResponse.ValidationErrors,
                Reason = paymentResponse.Reason,
                TimeStamp = DateTime.UtcNow
            };

            expectedPaymentResponse.Should().BeEquivalentTo(expectedPaymentResponse, x => x.Excluding(a => a.TimeStamp));
        }

        private MakePaymentCommand CreateMakePaymentCommand()
        {
            return new MakePaymentCommand
            {
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
                Amount = 100
            };
        }

        private static (Payment, PaymentResponse) CreateSuccessfulPaymentAndPaymentResponse()
        {
            var id = Guid.NewGuid().ToString();
            var bankPaymentId = Guid.NewGuid().ToString();
            var merchantPaymentId = Guid.NewGuid().ToString();
            var billingDetails = new BillingDetails
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
            };
            var currency = "GBP";
            var amount = 100;
            var status = "success";
            var validationErrors = new List<ValidationError>();
            var reason = "Payment fetched successfully";

            var payment = new Payment
            {
                Id = id,
                BankPaymentId = bankPaymentId,
                MerchantPaymentId = merchantPaymentId,
                BillingDetails = billingDetails,
                Currency = currency,
                Amount = amount,
                Status = status,
                ValidationErrors = validationErrors,
                Reason = reason
            };

            var paymentResponse = new PaymentResponse
            {
                Id = id,
                BankPaymentId = bankPaymentId,
                MerchantPaymentId = merchantPaymentId,
                BillingDetails = billingDetails,
                Currency = currency,
                Amount = amount,
                Status = status,
                ValidationErrors = validationErrors,
                Reason = reason
            };

            return (payment, paymentResponse);
        }

    }
}
