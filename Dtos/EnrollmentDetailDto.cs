using EnglishCentralManagement.Dtos.Pagination;

namespace EnglishCentralManagement.Dtos
{
    public class EnrollmentDetailDto
    {
        public long EnrollmentId { get; set; }
        public string ClassCode { get; set; }
        public string? CourseName { get; set; }
        public string StudentName { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public long? StudentId { get; set; }
        public long? ClassId { get; set; }
        public DateTimeOffset? StartDateClass { get; set; }
        public int? DurationCourse { get; set; }
        public PagedResult<PaymentScheduleDto>? PaymentSchedules { get; set; } = new PagedResult<PaymentScheduleDto>();

    }

    public class PaymentScheduleDto
    {
        public long Id { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }

    public class PaymentEditDto
    {
        public DateTimeOffset PaidAt { get; set; }
        public decimal PaidAmount { get; set; }
        public int PaymentMethod { get; set; }
    }
}
