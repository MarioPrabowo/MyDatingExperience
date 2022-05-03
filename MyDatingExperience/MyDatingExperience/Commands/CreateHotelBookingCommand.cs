using MediatR;
using MyDatingExperience.DTOs;
using MyDatingExperience.Repository;

namespace MyDatingExperience.Commands
{
    public class CreateHotelBookingCommand : IRequest<HotelBookingResponse>
    {
        public HotelBookingRequest Request { get; set; }

        public class Handler : IRequestHandler<CreateHotelBookingCommand, HotelBookingResponse>
        {
            private DateDbContext _dbContext;

            public Handler(DateDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<HotelBookingResponse> Handle(CreateHotelBookingCommand request, CancellationToken cancellationToken)
            {
                var entity = new HotelBookingEntity
                {
                    Name = request.Request.Name,
                    BookingDate = request.Request.BookingDate,
                    BookingId = Guid.NewGuid(),
                };
                _dbContext.Add(entity);
                await _dbContext.SaveChangesAsync();

                return new HotelBookingResponse
                {
                    BookingId = entity.BookingId,
                    Name = entity.Name,
                    BookingDate = entity.BookingDate,
                };
            }
        }
    }
}
