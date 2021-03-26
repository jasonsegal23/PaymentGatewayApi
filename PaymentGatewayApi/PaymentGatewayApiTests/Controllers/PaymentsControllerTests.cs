using AutoMapper;
using MediatR;
using Serilog;
using FakeItEasy;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using PaymentGateway.Controllers;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Responses;
using PaymentGateway.Commands;
using PaymentGateway.Models;
using PaymentGateway.Models.Billing;
using PaymentGateway.Validators;
using PaymentGateway.Dtos;

namespace PaymentGatewayTests.Controllers
{
    [TestFixture]
    class PaymentsControllerTests
    {
        private PaymentsController _sut;

        private readonly IMediator _mediator = A.Fake<IMediator>();
        private readonly IMapper _mapper = A.Fake<IMapper>();
        private readonly ILogger _logger = A.Fake<ILogger>();

        [SetUp]
        public void SetUp()
        {
            _sut = new PaymentsController(_mediator, _mapper, _logger);
        }

        [Test]
        public async Task GetPayment_Returns400BadRequest_WhenIdIsNull()
        {
            // Arrange
            string id = null;

            // Act
            var action = await _sut.GetPayment(id);
            var actionResult = action.Result as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(actionResult.StatusCode, 400);
        }

        [Test]
        public async Task GetPayment_Returns400BadRequest_WhenIdIsEmpty()
        {
            // Arrange
            string id = string.Empty;

            // Act
            var action = await _sut.GetPayment(id);
            var actionResult = action.Result as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(actionResult.StatusCode, 400);
        }

        [Test]
        public async Task GetPayment_Returns200OkWithResponse_WhenPaymentFetchedSuccessfully()
        {
            // Arrange
            var id = new Guid().ToString();
            var fakeSuccessPaymentResponse = CreateFakeSuccessPaymentResponse();

            A.CallTo(() => _mediator.Send(A<IRequest<PaymentResponse>>._, A<CancellationToken>._))
                .Returns(Task.FromResult(fakeSuccessPaymentResponse));

            // Act
            var action = await _sut.GetPayment(id);
            var actionResult = action.Result as OkObjectResult;
            var response = actionResult.Value;

            // Assert
            Assert.AreEqual(actionResult.StatusCode, 200);
            Assert.AreEqual(fakeSuccessPaymentResponse, response);
        }

        [Test]
        public async Task GetPayment_Returns404NotFound_WhenPaymentNotFound()
        {
            // Arrange
            var id = new Guid().ToString();
            var fakeFailurePaymentResponse = CreateFakeFetchFailurePaymentResponse();

            A.CallTo(() => _mediator.Send(A<IRequest<PaymentResponse>>._, A<CancellationToken>._))
                .Returns(Task.FromResult(fakeFailurePaymentResponse));

            // Act
            var action = await _sut.GetPayment(id);
            var actionResult = action.Result as NotFoundResult;

            // Assert
            Assert.AreEqual(actionResult.StatusCode, 404);
        }

        [Test]
        public async Task MakePayment_Returns201CreatedWithResponse_WhenPaymentMadeSuccessfully()
        {
            // Arrange
            var (makePaymentCommandDto, makePaymentCommand) = CreateFakeMakePaymentCommandAndDto();

            var fakeSuccessPaymentResponse = CreateFakeSuccessPaymentResponse();


            A.CallTo(() => _mapper.Map<MakePaymentCommand>(makePaymentCommandDto)).Returns(makePaymentCommand);

            A.CallTo(() => _mediator.Send(A<IRequest<PaymentResponse>>._, A<CancellationToken>._))
                .Returns(Task.FromResult(fakeSuccessPaymentResponse));

            // Act
            var action = _sut.MakePayment(makePaymentCommandDto);
            var actionResult = (CreatedResult)action.Result;
            var paymentResponseId = actionResult.Value
                .GetType().GetProperty("Id").GetValue(actionResult.Value).ToString();

            // Assert
            Assert.AreEqual(actionResult.StatusCode, 201);
            Assert.AreEqual(paymentResponseId, fakeSuccessPaymentResponse.Id.ToString());
        }

        [Test]
        public async Task MakePayment_Returns400BadRequest_WhenPaymentHasFailed()
        {
            // Arrange
            var (makePaymentCommandDto, makePaymentCommand) = CreateFakeMakePaymentCommandAndDto();

            var fakePaymentFailurePaymentResponse = CreateFakeMakePaymentFailurePaymentResponse();

            A.CallTo(() => _mapper.Map<MakePaymentCommand>(makePaymentCommandDto)).Returns(makePaymentCommand);

            A.CallTo(() => _mediator.Send(A<IRequest<PaymentResponse>>._, A<CancellationToken>._))
                .Returns(Task.FromResult(fakePaymentFailurePaymentResponse));

            // Act
            var action = _sut.MakePayment(makePaymentCommandDto);
            var actionResult = (BadRequestObjectResult)action.Result;
            var paymentResponseFailureReason = actionResult.Value
                .GetType().GetProperty("Reason").GetValue(actionResult.Value).ToString();

            // Assert
            Assert.AreEqual(actionResult.StatusCode, 400);
            Assert.AreEqual(fakePaymentFailurePaymentResponse.Reason, paymentResponseFailureReason);
        }

        private static PaymentResponse CreateFakeSuccessPaymentResponse()
        {
            return new PaymentResponse
            {
                Id = new Guid().ToString(),
                BankPaymentId = new Guid().ToString(),
                MerchantPaymentId = new Guid().ToString(),
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
                TimeStamp = DateTime.UtcNow
            };
        }

        private static PaymentResponse CreateFakeFetchFailurePaymentResponse()
        {
            return new PaymentResponse
            {
                Id = new Guid().ToString(),
                Status = "failure",
                Reason = "Failed to fetch payment"
            };
        }

        private static PaymentResponse CreateFakeMakePaymentFailurePaymentResponse()
        {
            return new PaymentResponse
            {
                Id = new Guid().ToString(),
                Status = "failure",
                Reason = "Failed to process payment",
                ValidationErrors = new List<ValidationError>()
            };
        }

        private static (MakePaymentCommandDto, MakePaymentCommand) CreateFakeMakePaymentCommandAndDto()
        {
            var merchantPaymentId = "15359b7d-4e92-4a02-b303-f7529277fe05";
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

            var makePaymentCommandDto = new MakePaymentCommandDto
            {
                MerchantPaymentId = merchantPaymentId,
                BillingDetails = billingDetails,
                Currency = currency,
                Amount = amount
            };

            var makePaymentCommand = new MakePaymentCommand
            {
                MerchantPaymentId = merchantPaymentId,
                BillingDetails = billingDetails,
                Currency = currency,
                Amount = amount
            };

            return (makePaymentCommandDto, makePaymentCommand);
        }
    }
}
