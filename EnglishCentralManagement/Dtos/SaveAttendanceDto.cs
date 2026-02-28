namespace EnglishCentralManagement.Dtos
{
    public class SaveAttendanceDto
    {
        public long SessionId { get; set; }
        public List<StudentAttendanceRequest> Students { get; set; }
    }

    public class StudentAttendanceRequest
    {
        public long StudentId { get; set; }
        public int Status { get; set; }
    }
}
