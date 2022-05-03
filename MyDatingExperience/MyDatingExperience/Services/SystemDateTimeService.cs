namespace MyDatingExperience.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
