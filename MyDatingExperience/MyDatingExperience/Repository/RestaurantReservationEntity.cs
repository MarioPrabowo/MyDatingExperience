namespace MyDatingExperience.Repository
{
    public class RestaurantReservationEntity
    {
        public Guid BookingId { get; set; }
        public string Name { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
