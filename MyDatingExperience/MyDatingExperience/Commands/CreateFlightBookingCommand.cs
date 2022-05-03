using MediatR;
using MyDatingExperience.DTOs;
using System.Globalization;

namespace MyDatingExperience.Commands
{
    public class CreateFlightBookingCommand : IRequest<FlightBookingResponse>
    {
        public FlightBookingRequest Request { get; set; }

        public class Handler : IRequestHandler<CreateFlightBookingCommand, FlightBookingResponse>
        {
            public async Task<FlightBookingResponse> Handle(CreateFlightBookingCommand request, CancellationToken cancellationToken)
            {
                // In real codebase, there will be a code to save data in DB here
                // For this example, we will jump straight to returning response object after saving data in DB
                return new FlightBookingResponse
                {
                    Name = request.Request.Name,
                    BookingDate = DateTime.Parse(request.Request.BookingDate, CultureInfo.GetCultureInfo("en-au")),
                };
            }
        }
    }
}
