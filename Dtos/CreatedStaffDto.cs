using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Dtos
{
    public class CreatedStaffDto
    {
        [Required, MaxLength(50)]
        public string? FirstName { get; set; }

        [Required, MaxLength(50)]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required, MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        public ContractType? ContractType { get; set; }
        public WorkingType? WorkingType { get; set; }

        [Range(0, 50)]
        public int? YearsOfExperience { get; set; }

        public decimal? HourlyRate { get; set; }
        public decimal? MonthlySalary { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset? OnboardingDate { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public GenderEnum? Gender { get; set; }
        public string? Title { get; set; }

        [Required, MaxLength(50)]
        public string? Username { get; set; }

        [Required, MaxLength(100)]
        public string? Password { get; set; }

        [Required]
        public RoleType? Role { get; set; }

        public IFormFile? Avatar { get; set; }
        public long? StaffId { get; set; }
    }
}
