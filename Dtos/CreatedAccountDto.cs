using EnglishCentralManagement.Models.Enum;

namespace EnglishCentralManagement.Dtos
{
    public class CreatedAccountDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public RoleType RoleId { get; set; }
        public long?  StaffId { get; set; }
        public long?  StudentId { get; set; }
    }
}
