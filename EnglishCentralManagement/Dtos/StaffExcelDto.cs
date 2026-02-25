namespace EnglishCentralManagement.Dtos
{
    public class StaffExcelDto
    {
        public string FullName { get; set; }
        public string? Email { get; set; }
        public string? ContractType { get; set; }
        public decimal? MonthlySalary { get; set; }
        public decimal? HourlyRate { get; set; }
        public string? PaymentCard { get; set; }
        public string? BankCard { get; set; }
        public int? WorkingHour { get; set; }
    }
}
