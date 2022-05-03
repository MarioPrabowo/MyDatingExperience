using MediatR;
using Microsoft.EntityFrameworkCore;
using MyDatingExperience.DTOs;
using MyDatingExperience.Repository;

namespace MyDatingExperience.Commands
{
    public class GetRestaurantReservationQuery : IRequest<RestaurantReservationsResponse?>
    {
        public Guid BookingId { get; set; }

        public class Handler : IRequestHandler<GetRestaurantReservationQuery, RestaurantReservationsResponse?>
        {
            private DateDbContext _dbContext;

            public Handler(DateDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<RestaurantReservationsResponse?> Handle(GetRestaurantReservationQuery request, CancellationToken cancellationToken)
            {
                var entity = await (
                    from b in _dbContext.RestaurantReservation
                    where b.BookingId == request.BookingId
                    select b
                ).AsNoTracking().FirstOrDefaultAsync();

                if(entity == null)
                {
                    return null;
                }

                return new RestaurantReservationsResponse
                {
                    BookingId = entity.BookingId,
                    Name = entity.Name,
                    BookingDate = entity.BookingDate,
                };
            }
        }
    }
}
