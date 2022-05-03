using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDatingExperience.Commands;
using MyDatingExperience.DTOs;

namespace MyDatingExperience.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HotelBookingsController : ControllerBase
    {
        public readonly IMediator _mediator;

        public HotelBookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public Task<HotelBookingResponse> CreateHotelBooking([FromBody] HotelBookingRequest request)
        {
            return _mediator.Send(new CreateHotelBookingCommand { Request = request });
        }

        [HttpGet("{bookingId}")]
        public Task<HotelBookingResponse?> GetHotelBooking([FromRoute] Guid bookingId)
        {
            return _mediator.Send(new GetHotelBookingQuery { BookingId = bookingId });
        }
    }
}
