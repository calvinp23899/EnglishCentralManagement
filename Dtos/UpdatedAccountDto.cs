using EnglishCentralManagement.Models.Enum;

namespace EnglishCentralManagement.Dtos
{
    public class UpdatedAccountDto
    {
        public long AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public RoleType RoleType { get; set; }
    }
}
