using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Dtos
{
    public class UpdatedStaffDto
    {
        [MaxLength(50, ErrorMessage = "{0} cannot exceed {1} characters")]
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "DateOfBirth is required")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(20, ErrorMessage = "{0} cannot exceed {1} characters")]
        [Required(ErrorMessage = "PhoneNumber is required")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "{0} invalid email address, should be ...@gmail.com")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "ContractType is required")]
        public ContractType? ContractType { get; set; }
        [Required(ErrorMessage = "WorkingType is required")]
        public WorkingType? WorkingType { get; set; }

        [Range(0, 50, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? YearsOfExperience { get; set; }
        [Required(ErrorMessage = "HourlyRate is required")]
        public decimal? HourlyRate { get; set; }
        public decimal? MonthlySalary { get; set; }
        [Range(0, 300, ErrorMessage = "{0} must be between {1} and {2}")]
        [Required(ErrorMessage = "WorkingHour is required")]
        public int? WorkingHour { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset? OnboardingDate { get; set; }
        [MaxLength(50, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string? Address { get; set; }
        [Required]
        public GenderEnum? Gender { get; set; }
        [MaxLength(10, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string? Title { get; set; }
        [MaxLength(100, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public RoleType? Role { get; set; }

        public IFormFile? Avatar { get; set; }
        public long? StaffId { get; set; }
        [MaxLength(30, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string? PaymentCard { get; set; }
        [MaxLength(30, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string? BankCard { get; set; }
    }
}
