namespace EnglishCentralManagement.Dtos
{
    public class StudentClassDto
    {
        public long? ClassId { get; set; }
        public long? ClassName { get; set; }
        public DateTimeOffset? StartedDate { get; set; }
        public DateTimeOffset? EndedDate { get; set; }
        public DateTimeOffset? StudentJoinedDate { get; set; }
        public int? LessonAttended { get; set; }
        public string? StudentStatusInClass { get; set; }
    }
}
