using EnglishCentralManagement.Dtos;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IPaymentService
    {
        Task AddStudentInClass(long studentId, long classId);
        Task EditPayment(long paymentScheduleId, PaymentEditDto model);
    }
}
