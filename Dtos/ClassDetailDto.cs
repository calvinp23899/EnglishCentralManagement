using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Models.Enum;

namespace EnglishCentralManagement.Dtos
{
    public class ClassDetailDto 
    {
        public string? TeacherName { get; set; }
        public string? CourseName { get; set; }
        public string? ClassCode { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        public int? MaxStudents { get; set; }

        public ClassStatusEnum? Status { get; set; }

        public string? Note { get; set; }
        public long? ClassId { get; set; }
        public long? TeacherId { get; set; }
        public long? CourseId { get; set; }
        public PagedResult<StudentListDto>? StudentsInClass { get; set; } = new PagedResult<StudentListDto>();
        public PagedResult<StudentListDto>? StudentsAddToClass { get; set; } = new PagedResult<StudentListDto>();

    }

    public class StudentListDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public long? EnrollmentId { get; set; }
        public string? EnrollmentStatus { get; set; }

    }
}
