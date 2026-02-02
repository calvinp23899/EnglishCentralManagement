using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Dtos
{
    public class CreatedStudentDto
    {
        [Required, MaxLength(50)]
        public string? FirstName { get; set; }

        [Required, MaxLength(50)]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public GenderEnum? Gender { get; set; }

        [Required, MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Address { get; set; }
        public StudentStatus? Status { get; set; }

        [Required, MaxLength(50)]
        public string? Username { get; set; }

        [Required, MaxLength(50)]
        public string? Password { get; set; }

        [Required]
        public RoleType? Role { get; set; }

        public long? StudentId { get; set; }

        public List<StudentClassDto>? StudentClasses { get; set; } = new List<StudentClassDto>();

    }
}
