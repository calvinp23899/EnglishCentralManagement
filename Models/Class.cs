using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Models
{
    public class Class : BaseModel
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = null!;

        [Required]
        public long CourseId { get; set; }

        [Required]
        public long StaffId { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        [Range(1, 100)]
        public int? MaxStudents { get; set; }

        public ClassStatusEnum? Status { get; set; }

        public string? Note { get; set; }

        public Course Course { get; set; } = null!;
        public Staff Staff { get; set; } = null!;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
