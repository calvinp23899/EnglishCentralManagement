using System.Data;

namespace EnglishCentralManagement.Models
{
    public class Account : BaseModel
    {
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        public long? StudentId { get; set; }
        public Student? Student { get; set; }

        public long? StaffId { get; set; }
        public Staff? Staff { get; set; }
        public long? RoleId { get; set; }
        public Role Role { get; set; }
    }
}
