namespace EnglishCentralManagement.Dtos
{
    public class StudentAttendance
    {
        public long? AttendanceId { get; set; }
        public long? StudentId { get; set; }
        public string FullName { get; set; }
        public int Status { get; set; }
    }
}
