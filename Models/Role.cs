using EnglishCentralManagement.Models.Enum;

namespace EnglishCentralManagement.Models
{
    public class Role : BaseModel
    {
        public RoleType Name { get; set; }
        public ICollection<Account> Accounts { get; set; } = new List<Account>();

    }
}
