namespace EnglishCentralManagement.Dtos.Expense
{
    public class ExpensePieChartDto
    {
        public List<string> Labels { get; set; } = new();
        public List<decimal> Data { get; set; } = new();
    }
}
