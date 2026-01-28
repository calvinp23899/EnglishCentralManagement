using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCentralManagement.Models
{
    public class Teacher : BaseModel
    {
        [MaxLength(20)]
        public string? FirstName { get; set; }

        [MaxLength(20)]
        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? Address { get; set; }

        public string? AvatarUrl { get; set; }

        public TeacherStatus? Status { get; set; } = TeacherStatus.Active;

        public int? YearsOfExperience { get; set; }

        public ContractType? ContractType { get; set; }

        public WorkingType? WorkingType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? HourlyRate { get; set; }

        public decimal? Salary { get; set; }

        public DateTime? OnboardingDate { get; set; }


    }
}
