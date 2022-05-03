namespace MyDatingExperience.DTOs
{
    public class SendInvoiceResponse
    {
        public Guid InvoiceId { get; set; }
        public string Sender { get; set; }
        public decimal Amount { get; set; }
        public DateTime SentDate { get; set; }
    }
}
