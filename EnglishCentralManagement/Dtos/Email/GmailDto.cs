namespace EnglishCentralManagement.Dtos.Email
{
    public class GmailDto
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
        //Invoice
        public string? InvoiceId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal? ProductAmount { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
