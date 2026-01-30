using EnglishCentralManagement.Models.Enum;

namespace EnglishCentralManagement.Dtos
{
    public class StaffDetailDto
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public ContractType? ContractType { get; set; }
        public WorkingType? WorkingType { get; set; }
        public int? YearsOfExperience { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal? MonthlySalary { get; set; }
        public DateTimeOffset? OnboardingDate { get; set; }
        public string? Address { get; set; }
        public string? Title { get; set; }
        public string Image { get; set; }

    }
}
