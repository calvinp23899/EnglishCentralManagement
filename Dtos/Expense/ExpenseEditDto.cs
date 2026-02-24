namespace EnglishCentralManagement.Dtos.Expense
{
    public class ExpenseEditDto
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public int Category { get; set; }
        public int Type { get; set; }
        public string ExpenseDate { get; set; }
        public string? Note { get; set; }
        public long? ClassId { get; set; }

    }
}
