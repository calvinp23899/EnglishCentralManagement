using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Dtos
{
    public class CreateSessionDto
    {
        [Required(ErrorMessage = "SessionName is required")]
        public string SessionName { get; set; }

        [Required(ErrorMessage = "SessionDate is required")]
        public DateTimeOffset SessionDate { get; set; }

        [Required(ErrorMessage = "StartTime is required")]
        public TimeOnly StartTime { get; set; }

        [Required(ErrorMessage = "EndTime is required")]
        public TimeOnly EndTime { get; set; }

        public SessionStatusEnum Status { get; set; } = SessionStatusEnum.Scheduled;

        public string? Note { get; set; }
        public string? FeedBack { get; set; }
        public long? ClassId { get; set; }
    }
}
