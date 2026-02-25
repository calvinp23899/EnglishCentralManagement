using EnglishCentralManagement.Models.Enum;

namespace EnglishCentralManagement.Models
{
    public class Payment : BaseModel
    {
        public long PaymentScheduleId { get; set; }
        public PaymentSchedule PaymentSchedule { get; set; } = null!;
        public DateTimeOffset PaidAt { get; set; }
        public decimal PaidAmount { get; set; }
        public PaymentMethod Method { get; set; }
    }
}
