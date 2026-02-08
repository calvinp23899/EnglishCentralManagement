using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Models
{
    public class PaymentSchedule : BaseModel
    {
        [Required]
        public long EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;

        public DateTimeOffset DueDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 100000000)]
        public decimal Amount { get; set; }

        public PaymentScheduleStatus Status { get; set; } = PaymentScheduleStatus.Pending;

        public Payment? Payment { get; set; }
    }
}
