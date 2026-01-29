using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Models
{
    public class Enrollment : BaseModel
    {
        [Required]
        public long StudentId { get; set; }

        [Required]
        public long ClassId { get; set; }

        public DateTimeOffset EnrolledAt { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

        public ICollection<PaymentSchedule> PaymentSchedules { get; set; } = new List<PaymentSchedule>();
    }
}
