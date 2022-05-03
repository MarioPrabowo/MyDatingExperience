using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDatingExperience.Commands;
using MyDatingExperience.DTOs;

namespace MyDatingExperience.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoicesController : ControllerBase
    {
        public readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public Task<SendInvoiceResponse> SendInvoice([FromBody] SendInvoiceRequest request)
        {
            return _mediator.Send(new SendInvoiceCommand { Request = request });
        }
    }
}
