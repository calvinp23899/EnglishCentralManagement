namespace EnglishCentralManagement.Models
{
    public class Role : BaseModel
    {
        public string Name { get; set; }
        public ICollection<Account> Accounts { get; set; } = new List<Account>();

    }
}
