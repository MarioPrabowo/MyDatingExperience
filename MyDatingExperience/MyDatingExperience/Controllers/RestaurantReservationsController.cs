using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDatingExperience.Commands;
using MyDatingExperience.DTOs;

namespace MyDatingExperience.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RestaurantReservationsController : ControllerBase
    {
        public readonly IMediator _mediator;

        public RestaurantReservationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public Task<RestaurantReservationsResponse> CreateReservation([FromBody] RestaurantReservationsRequest request)
        {
            return _mediator.Send(new CreateRestaurantReservationCommand { Request = request });
        }

        [HttpGet("{bookingId}")]
        public Task<RestaurantReservationsResponse?> GetReservation([FromRoute] Guid bookingId)
        {
            return _mediator.Send(new GetRestaurantReservationQuery { BookingId = bookingId });
        }
    }
}
