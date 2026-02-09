namespace EnglishCentralManagement.Dtos
{
    public class DashboardDto
    {
        public int CountClass { get; set; }
        public int CountStudent { get; set; }
        public int CountStaff { get; set; }
        public decimal RevenueThisMonth { get; set; }
        //public List<RevenueByMonthDto> ChartData = new List<RevenueByMonthDto>();
    }
    public class RevenueByMonthDto
    {
        public int Month { get; set; }
        public decimal Total { get; set; }
    }
}
