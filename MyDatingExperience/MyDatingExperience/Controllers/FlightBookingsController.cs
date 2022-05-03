using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDatingExperience.Commands;
using MyDatingExperience.DTOs;

namespace MyDatingExperience.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightBookingsController : ControllerBase
    {
        public readonly IMediator _mediator;

        public FlightBookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public Task<FlightBookingResponse> CreateFlightBooking([FromBody] FlightBookingRequest request)
        {
            return _mediator.Send(new CreateFlightBookingCommand { Request = request});
        }
    }
}