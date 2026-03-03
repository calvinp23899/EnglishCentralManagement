using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Dtos
{
    public class UpdateStudentDto
    {
        [MaxLength(50, ErrorMessage = "{0} cannot exceed {1} characters")]
        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "{0} cannot exceed {1} characters")]
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public GenderEnum? Gender { get; set; }

        [MaxLength(20, ErrorMessage = "{0} cannot exceed {1} characters")]
        [Required(ErrorMessage = "Phone Number is required")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Email is invalid format")]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }
        public StudentStatus? Status { get; set; }

        [MaxLength(50, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string? Password { get; set; }

        public long? StudentId { get; set; }
    }
}
