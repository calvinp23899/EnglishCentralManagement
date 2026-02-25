using EnglishCentralManagement.Dtos.Pagination;

namespace EnglishCentralManagement.Dtos.Expense
{
    public class ExpenseDto
    {
        public long Id { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal Amount { get; set; }
    }

    public class ExpenseTabDto
    {
        public string? DataChart { get; set; }
        public PagedResult<ExpenseDto> Expenses { get; set; } = new PagedResult<ExpenseDto>();
    }
}
