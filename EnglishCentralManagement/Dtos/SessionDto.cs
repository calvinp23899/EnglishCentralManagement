using EnglishCentralManagement.Models.Enum;

namespace EnglishCentralManagement.Dtos
{
    public class SessionDto
    {
        public long? Id { get; set; }
        public string? SessionName { get; set; }
        public DateTimeOffset? SessionDate { get; set; }
        public string? Attendance { get; set; }
        public string? SessionStatus { get; set; }
        public string? Note { get; set; }
        public string? FeedBack { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public SessionStatusEnum? SessionStatusEnum { get; set; }


    }
}
