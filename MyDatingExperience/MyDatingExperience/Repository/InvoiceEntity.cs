namespace MyDatingExperience.Repository
{
    public class InvoiceEntity
    {
        public Guid InvoiceId { get; set; }
        public string Sender { get; set; }
        public decimal Amount { get; set; }
        public DateTime SentDate { get; set; }
    }
}
