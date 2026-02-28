using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Dtos
{
    public class UpdateSessionDto
    {
        [Required]
        public string SessionName { get; set; }

        [Required]
        public DateTimeOffset SessionDate { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        public SessionStatusEnum Status { get; set; } = SessionStatusEnum.Scheduled;

        public string? Note { get; set; }
        public string? FeedBack { get; set; }
        public long? ClassId { get; set; }
        public long? Id { get; set; }
    }
}
