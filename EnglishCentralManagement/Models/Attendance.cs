using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Models
{
    public class Attendance : BaseModel
    {
        [Required]
        public long ClassSessionId { get; set; }
        public ClassSession ClassSession { get; set; } = null!;

        [Required]
        public long EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;

        public AttendanceStatusEnum Status { get; set; }

        public string? Note { get; set; }

    }
}
