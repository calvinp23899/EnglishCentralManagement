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
            var countMaxPayment = await _context.PaymentSchedules.CountAsync();

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
                Status = EnrollmentStatus.Active,
                CreatedDate = DateTimeOffset.UtcNow.ToUniversalTime(),
            };
            _context.Enrollments.Add(data);
            await _context.SaveChangesAsync();
            var paymentSchedules = new List<PaymentSchedule>();
            for (int month = 0; month < classModel.Course.DurationInMonths; month++)
            {
                var dueDate = classModel.StartDate.AddMonths(month);
                paymentSchedules.Add(new PaymentSchedule
                {
                    Enrollment = data,
                    DueDate = dueDate,
                    Amount = classModel.Course.MonthlyFee,
                    Status = PaymentScheduleStatus.Pending,
                    CreatedBy = _currentUser.FullName,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                    PaymenCode = string.Join("-", "TF", data.ClassId, data.StudentId, countMaxPayment),
                    Title = string.Join(" ", "Thu Phí Lần", month + 1)
                });
            }
            _context.PaymentSchedules.AddRange(paymentSchedules);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalRevenueThisMonth()
        {
            var now = DateTimeOffset.UtcNow;

            var startOfMonth = new DateTimeOffset(
                now.Year, now.Month, 1,
                0, 0, 0,
                TimeSpan.Zero
            );

            var startOfNextMonth = startOfMonth.AddMonths(1);

            var total = await _context.PaymentSchedules
                .Where(x =>
                    x.Status == PaymentScheduleStatus.Paid &&
                    x.DueDate >= startOfMonth &&
                    x.DueDate < startOfNextMonth
                )
                .SumAsync(x => x.Amount);
            return total;
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

        public async Task<List<RevenueByMonthDto>> GetDataChart()
        {
            int year = DateTime.Now.Year;

            var rawData = await _context.Payments
                .Where(x =>
                    x.PaymentSchedule.Status == PaymentScheduleStatus.Paid &&
                    x.PaymentSchedule.DueDate.Year == year)
                .GroupBy(x => x.PaymentSchedule.DueDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(x => x.PaymentSchedule.Amount)
                })
                .ToListAsync();

            var result = Enumerable.Range(1, 12)
                .Select(month => new RevenueByMonthDto
                {
                    Month = month,
                    Total = rawData.FirstOrDefault(x => x.Month == month)?.Total ?? 0
                })
                .ToList();

            return result;
        }
    }
}
