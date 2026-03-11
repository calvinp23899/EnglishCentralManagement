namespace EnglishCentralManagement.Dtos
{
    public class AccountDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public long? StaffId { get; set; }

    }
}
