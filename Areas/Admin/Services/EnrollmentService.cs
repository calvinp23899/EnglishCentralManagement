using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models;
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

        public async Task AddStudentInClassAsync(long studentId, long classId)
        {
            var classModel = await _context.Classes
                .Where(x => x.Id == classId && !x.IsDeleted).FirstOrDefaultAsync();
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
            classModel.MaxStudents++;
            _context.Enrollments.Add(data);
            _context.Classes.Update(classModel);
            await _context.SaveChangesAsync();
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
