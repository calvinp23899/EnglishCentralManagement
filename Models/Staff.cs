using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishCentralManagement.Models
{
    public class Staff : BaseModel
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        [MaxLength(255)]
        public string? AvatarUrl { get; set; }

        public TeacherStatus Status { get; set; } = TeacherStatus.Active;

        [Range(0, 50)]
        public int? YearsOfExperience { get; set; }

        public ContractType? ContractType { get; set; }
        public WorkingType? WorkingType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 10000000)]
        public decimal? HourlyRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 1000000000)]
        public decimal? MonthlySalary { get; set; }

        public DateTimeOffset? OnboardingDate { get; set; }

        [MaxLength(20)]
        public string? Title { get; set; }

        public bool? Gender { get; set; }
        public int? ContractPeriod { get; set; }
        //CardNumber
        public string? PaymentCard { get; set; }
        //Bank - TP, BIDV, ACB
        public string? BankCard { get; set; }
        public int? WorkingHour { get; set; }

        public ICollection<Class> Classes { get; set; } = new List<Class>();
        public ICollection<EventCalendar> Events { get; set; } = new List<EventCalendar>();

    }
}
