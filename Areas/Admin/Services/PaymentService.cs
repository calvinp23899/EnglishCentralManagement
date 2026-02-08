using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Extensions;
using EnglishCentralManagement.Models;
using EnglishCentralManagement.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly EnglishCentreDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public PaymentService(EnglishCentreDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task AddStudentInClass(long studentId, long classId)
        {
            var classModel = await _context.Classes
                .Include(x => x.Course)
                .Where(x => x.Id == classId && !x.IsDeleted).FirstOrDefaultAsync();
            if (classModel == null)
                throw new Exception("Class not found");
            var currentStudents = await _context.Enrollments
                .CountAsync(x => x.ClassId == classId && x.Status == EnrollmentStatus.Active);

            if (classModel.MaxStudents.HasValue &&
                currentStudents >= classModel.MaxStudents.Value)
            {
                throw new Exception("Class is full");
            }
            var data = new Enrollment
            {
                ClassId = classId,
                StudentId = studentId,
                CreatedBy = _currentUser.FullName,
                EnrolledAt = DateTime.UtcNow.ToUniversalTime(),
                StartDate = DateTime.UtcNow.ToUniversalTime(),
                EndDate = classModel.EndDate,
                LessonAttended = 0,
                Status = EnrollmentStatus.Active,
                CreatedDate = DateTimeOffset.UtcNow.ToUniversalTime(),
            };
            _context.Enrollments.Add(data);
            await _context.SaveChangesAsync();
            var paymentSchedules = new List<PaymentSchedule>();
            for (int month = 0; month < classModel.Course.DurationInMonths; month++)
            {
                var dueDate = data.StartDate.AddMonths(month);
                paymentSchedules.Add(new PaymentSchedule
                {
                    Enrollment = data,
                    DueDate = dueDate,
                    Amount = classModel.Course.MonthlyFee,
                    Status = PaymentScheduleStatus.Pending,
                    CreatedBy = _currentUser.FullName,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                });
            }
            _context.PaymentSchedules.AddRange(paymentSchedules);
            await _context.SaveChangesAsync();
        }

        public async Task EditPayment(long paymentScheduleId, PaymentEditDto model)
        {
            var data = new Payment
            {
                PaymentScheduleId = paymentScheduleId,
                PaidAt = model.PaidAt.ToUtcDb(),
                PaidAmount = model.PaidAmount,
                CreatedBy = _currentUser.FullName,
                CreatedDate = model.PaidAt.ToUtcDb(),
                Method = (PaymentMethod)model.PaymentMethod
            };
            _context.Payments.Add(data);
            var paymentScheduleModel = await _context.PaymentSchedules
                .Where(x => x.Id == paymentScheduleId)
                .FirstOrDefaultAsync();
            if (paymentScheduleModel == null)
                throw new Exception("PaymentSchedule not found");
            paymentScheduleModel.Status = PaymentScheduleStatus.Paid;
            await _context.SaveChangesAsync();
        }
    }
}
