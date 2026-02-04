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
        public PagedResult<StudentListDto>? Students { get; set; } = new PagedResult<StudentListDto>();

    }

    public class StudentListDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
    }
}
