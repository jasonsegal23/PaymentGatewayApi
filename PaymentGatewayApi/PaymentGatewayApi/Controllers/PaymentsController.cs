using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Commands;
using PaymentGateway.Dtos;
using PaymentGateway.Queries;
using PaymentGateway.Responses;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PaymentsController(IMediator mediator, IMapper mapper, ILogger logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentResponse>> GetPayment(string id)
        {
            // Validate input argument
            if(id == null || id == string.Empty)
            {
                return BadRequest(new { Reason = "'id' field cannot be null or empty" });
            }

            var response = await _mediator.Send(new GetPaymentByIdQuery(id), new CancellationToken());

            if (response.Status.Equals("success"))
            {
                _logger.Information("{Timestamp} {Message}", DateTime.UtcNow.ToString(),
                    response.Reason);

                return Ok(response);
            }
            else
            {
                _logger.Information("{Timestamp} {Message}", DateTime.UtcNow.ToString(),
                    response.Reason);

                return NotFound();
            }  
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment([FromBody] MakePaymentCommandDto commandDto)
        {
            var command = _mapper.Map<MakePaymentCommand>(commandDto);
            var response = await _mediator.Send(command);

            if (response.Status.Equals("success"))
            {
                _logger.Information("{Timestamp} {EventId} {Message}", DateTime.UtcNow.ToString(),
                    response.Id.ToString(), "Payment successful");

                return Created(nameof(MakePaymentCommand), new { response.Id });
            }
            else
            {
                if(response.ValidationErrors.Count > 0)
                {
                    _logger.Information("{Timestamp} {EventId} {Message}", DateTime.UtcNow.ToString(),
                        response.Reason);   
                }
                else
                {
                    _logger.Information("{Timestamp} {EventId} {Message}", DateTime.UtcNow.ToString(),
                        response.Id.ToString(), response.Reason);
                }

                // Not quite sure what HTTP status code should be used for a failed request?
                return BadRequest(new { response.Reason });
            }

            
        }
    }
}
