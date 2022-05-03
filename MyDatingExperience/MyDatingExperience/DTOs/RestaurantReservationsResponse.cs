namespace MyDatingExperience.DTOs
{
    public class RestaurantReservationsResponse
    {
        public Guid BookingId { get; set; }
        public string Name { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
