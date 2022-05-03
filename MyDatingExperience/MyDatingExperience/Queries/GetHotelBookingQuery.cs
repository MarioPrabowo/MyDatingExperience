using MediatR;
using Microsoft.EntityFrameworkCore;
using MyDatingExperience.DTOs;
using MyDatingExperience.Repository;

namespace MyDatingExperience.Commands
{
    public class GetHotelBookingQuery : IRequest<HotelBookingResponse?>
    {
        public Guid BookingId { get; set; }

        public class Handler : IRequestHandler<GetHotelBookingQuery, HotelBookingResponse?>
        {
            private DateDbContext _dbContext;

            public Handler(DateDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<HotelBookingResponse?> Handle(GetHotelBookingQuery request, CancellationToken cancellationToken)
            {
                var entity = await (
                    from b in _dbContext.HotelBooking
                    where b.BookingId == request.BookingId
                    select b
                ).AsNoTracking().FirstOrDefaultAsync();

                if(entity == null)
                {
                    return null;
                }

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
