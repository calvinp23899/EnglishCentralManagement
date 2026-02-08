using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly EnglishCentreDbContext _context;
        private readonly ICurrentUserService _currentUser;


        public EnrollmentService(EnglishCentreDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<EnrollmentDto>> GetAllAsync(int pageIndex, int pageSize)
        {
            var query = _context.Enrollments
                .Include(x => x.Class)
                .Include(x => x.Student)
                .Where(e => e.IsDeleted == false && (e.Status != EnrollmentStatus.InActive || e.Status != EnrollmentStatus.Cancelled));

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(e => e.StudentId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new EnrollmentDto
                {
                    Id = e.Id,
                    StudentName = string.Join(" ", e.Student.FirstName, e.Student.LastName),
                    ClassCode = e.Class.Code,
                    PhoneNumber = e.Student.PhoneNumber,
                    Status = e.Status.GetDisplayEnumName()
                })
                .ToListAsync();

            return new PagedResult<EnrollmentDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<EnrollmentDetailDto> GetDetailById(long id, int pageIndex, int pageSize)
        {
            var model = await _context.Enrollments
                .Include(x => x.Class)
                    .ThenInclude(c => c.Course)
                .Include(x => x.Student)
                .Include(x => x.PaymentSchedules)
                .Where(e => e.Id == id && e.IsDeleted == false)
                .FirstOrDefaultAsync();
            var totalRecords = model.PaymentSchedules.Count();

            var items = model.PaymentSchedules
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new PaymentScheduleDto
                {
                    Id = e.Id,
                    DueDate = e.DueDate,
                    Amount = e.Amount,
                    Status = e.Status.GetDisplayEnumName(),
                })
                .ToList();
            var paymentScheduleList = new PagedResult<PaymentScheduleDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
            var data = new EnrollmentDetailDto
            {
                EnrollmentId = model.Id,
                CourseName = model.Class.Course.Name,
                DurationCourse = model.Class.Course.DurationInMonths,
                StartDateClass = model.Class.StartDate,
                ClassCode = model.Class.Code,
                ClassId = model.Class.Id,
                StudentId = model.Student.Id,
                StudentName = string.Join(" ", model.Student.FirstName, model.Student.LastName),
                PhoneNumber = model.Student.PhoneNumber,
                Status = model.Status.GetDisplayEnumName(),
                PaymentSchedules = paymentScheduleList
            };
            return data;
        }

        public async Task<PagedResult<StudentListDto>> GetStudentInClassAsync(int pageIndex, int pageSize, long classId)
        {
            var query = _context.Enrollments
                .Where(e => e.ClassId == classId && e.IsDeleted == false && (e.Status != EnrollmentStatus.InActive || e.Status != EnrollmentStatus.Cancelled));

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(e => e.StudentId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new StudentListDto
                {
                    Id = e.Student.Id,
                    FullName = string.Join(" ", e.Student.FirstName, e.Student.LastName),
                    Email = e.Student.Email,
                    PhoneNumber = e.Student.PhoneNumber,
                    Status = e.Student.Status.GetDisplayEnumName(),
                    EnrollmentId = e.Id,
                    EnrollmentStatus = e.Status.GetDisplayEnumName()
                })
                .ToListAsync();

            return new PagedResult<StudentListDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }


    }
}
