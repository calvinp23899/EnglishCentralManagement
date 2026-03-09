namespace EnglishCentralManagement.Dtos.Invoice
{
    public class TuitionReceiptDto
    {
        public string ReceiptNumber { get; set; }
        public DateTime Date { get; set; }

        public string? StudentName { get; set; }
        public string? ClassName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public decimal Total => Items.Sum(x => x.Amount);
        public List<TuitionReceiptItemDto> Items { get; set; } = new();

    }
    public class TuitionReceiptItemDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
