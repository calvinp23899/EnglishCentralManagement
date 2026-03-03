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
        public decimal? MonthlySalary
        {
            get
            {
                var total = HourlyRate * WorkingHour;
                return total;
            }
        }
        public DateTimeOffset? OnboardingDate { get; set; }
        public string? Address { get; set; }
        public string? Title { get; set; }
        public string Image { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public RoleType? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? WorkingHour { get; set; }
        public string? CardNumber { get; set; }
        public string? Bank { get; set; }
        public IFormFile Avatar { get; set; }

        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
