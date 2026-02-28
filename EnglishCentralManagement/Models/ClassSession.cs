using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Models
{
    public class ClassSession : BaseModel
    {
        [Required]
        public string SessionName { get; set; }
        [Required]
        public long ClassId { get; set; }
        public Class Class { get; set; } = null!;

        [Required]
        public DateTimeOffset SessionDate { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        public SessionStatusEnum Status { get; set; } = SessionStatusEnum.Scheduled;

        public string? Note { get; set; }
        public string? FeedBack { get; set; }

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
