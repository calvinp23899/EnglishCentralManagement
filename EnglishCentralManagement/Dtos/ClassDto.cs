namespace EnglishCentralManagement.Dtos
{
    public class ClassDto
    {
        public long ClassId { get; set; }
        public string? ClassCourse { get; set; }
        public string? ClassCode { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int? StudentCount { get; set; }
        public string? Status { get; set; }
    }
}
