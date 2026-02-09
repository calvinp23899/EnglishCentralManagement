namespace EnglishCentralManagement.Dtos
{
    public class ProfileDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string? Role { get; set; }
        public string? Title { get; set; }
        public string? ContractType { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;

                if (DateOfBirth.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }

    }
}
