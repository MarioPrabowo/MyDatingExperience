using MediatR;
using MyDatingExperience.DTOs;
using MyDatingExperience.Repository;

namespace MyDatingExperience.Commands
{
    public class CreateRestaurantReservationCommand : IRequest<RestaurantReservationsResponse>
    {
        public RestaurantReservationsRequest Request { get; set; }

        public class Handler : IRequestHandler<CreateRestaurantReservationCommand, RestaurantReservationsResponse>
        {
            private DateDbContext _dbContext;

            public Handler(DateDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<RestaurantReservationsResponse> Handle(CreateRestaurantReservationCommand request, CancellationToken cancellationToken)
            {
                var entity = new RestaurantReservationEntity
                {
                    Name = request.Request.Name,
                    BookingDate = request.Request.BookingDate,
                    BookingId = Guid.NewGuid(),
                };
                _dbContext.Add(entity);
                await _dbContext.SaveChangesAsync();

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
