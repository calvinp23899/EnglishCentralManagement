using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Models
{
    public class Student : BaseModel
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(200)]
        public string? Address { get; set; }

        public bool? Gender { get; set; }
        public string? Note { get; set; }

        public StudentStatus Status { get; set; } = StudentStatus.Active;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
