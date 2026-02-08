using EnglishCentralManagement.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Dtos
{
    public class CreatedClassDto
    {
        [Required]
        public long TeacherId { get; set; }
        [Required]
        public long CourseId { get; set; }
        [Required]
        public string ClassCode { get; set; }
        [Required]
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public int? MaxStudents { get; set; }

        public ClassStatusEnum? Status { get; set; }

        public string? Note { get; set; }
        public string? TeacherName { get; set; }
        public string? CourseName { get; set; }
        public long? ClassId { get; set; }

        public List<TeacherSelectDto>? Teachers { get; set; } = new List<TeacherSelectDto>();
        public List<CourseSelectDto>? Courses { get; set; } = new List<CourseSelectDto>();

    }


    public class TeacherSelectDto
    {
        public long Id { get; set; }
        public string FullName { get; set; } = null!;
    }

    public class CourseSelectDto
    {
        public long Id { get; set; }
        public string CourseName { get; set; } = null!;
    }
}
