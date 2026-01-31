namespace EnglishCentralManagement.Dtos
{
    public class StaffDto
    {
        public long Id { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

    }
}
