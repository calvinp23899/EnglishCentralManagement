namespace EnglishCentralManagement.Dtos.Profit
{
    public class CompareChartDto
    {
        public List<string> Labels { get; set; }
        public List<decimal> RevenueData { get; set; }
        public List<decimal> ExpenseData { get; set; }
    }
}
