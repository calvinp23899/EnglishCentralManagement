using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Extensions;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models;
using EnglishCentralManagement.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class ClassService : IClassService
    {
        private readonly EnglishCentreDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public ClassService(EnglishCentreDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<int> CountClass()
        {
            int countClass = await _context.Classes.CountAsync();
            return countClass;
        }

        public async Task CreateAsync(CreatedClassDto newClass)
        {
            var courseModel = await _context.Courses
                .Where(x => x.Id == newClass.CourseId)
                .FirstOrDefaultAsync();
            int DurationInMonths = courseModel?.DurationInMonths ?? 0;
            var data = new Class
            {
                Code = newClass.ClassCode,
                StartDate = newClass.StartDate.Value.ToUtcDb(),
                EndDate = newClass.StartDate.Value.AddMonths(DurationInMonths).ToUtcDb(),
                MaxStudents = newClass.MaxStudents,
                StaffId = newClass.TeacherId,
                CourseId = newClass.CourseId,
                Note = newClass.Note,
                Status = newClass.Status,
                CreatedDate = DateTimeOffset.UtcNow,
                CreatedBy = _currentUser.FullName,
            };

            _context.Classes.Add(data);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ClassDto>> GetAllAsync(int pageIndex, int pageSize)
        {
            var query = _context.Classes
                .Include(x => x.Course)
                .Where(x => !x.IsDeleted);

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ClassDto
                {
                    ClassId = x.Id,
                    ClassCourse = x.Course.Name,
                    ClassCode = x.Code,
                    StartDate = x.StartDate.ToVnTime(),
                    EndDate = x.EndDate.ToVnTime(),
                    StudentCount = x.MaxStudents,
                    Status = x.Status != null ? Common.GetDisplayEnumName(x.Status) : null
                })
                .ToListAsync();
            return new PagedResult<ClassDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<PagedResult<ClassDto>> GetAllMyClassAsync(int pageIndex, int pageSize, long teacherId)
        {
            var query = _context.Classes
               .Include(x => x.Course)
               .Where(x => !x.IsDeleted && x.StaffId == teacherId);

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ClassDto
                {
                    ClassId = x.Id,
                    ClassCourse = x.Course.Name,
                    ClassCode = x.Code,
                    StartDate = x.StartDate.ToVnTime(),
                    EndDate = x.EndDate.ToVnTime(),
                    StudentCount = x.MaxStudents,
                    Status = x.Status != null ? Common.GetDisplayEnumName(x.Status) : null
                })
                .ToListAsync();
            return new PagedResult<ClassDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<CreatedClassDto> GetClassInfoForEdit(long classId, List<TeacherSelectDto> listTeacherSelect, List<CourseSelectDto> listCourseSelect)
        {

            var classInfo = await GetDetailById(classId);

            var data = new CreatedClassDto();
            data.Teachers = listTeacherSelect;
            data.Courses = listCourseSelect;
            data.TeacherId = (long)classInfo.TeacherId;
            data.CourseId = (long)classInfo.CourseId;
            data.ClassCode = classInfo.ClassCode;
            data.StartDate = classInfo.StartDate; //already VNTime
            data.EndDate = classInfo.EndDate.Value;
            data.Note = classInfo.Note;
            data.Status = classInfo.Status;
            data.CourseName = classInfo.CourseName;
            data.TeacherName = classInfo.TeacherName;
            data.ClassId = classInfo.ClassId;
            data.MaxStudents = classInfo.MaxStudents;
            return data;
        }

        public async Task<ClassDetailDto> GetDetailById(long id)
        {
            var model = await _context.Classes
                .Include(x => x.Course)
                .Include(x => x.Staff)
                .Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            var currentStudents = await _context.Enrollments
                .CountAsync(x => x.ClassId == id && x.Status == EnrollmentStatus.Active);
            var data = new ClassDetailDto
            {
                ClassCode = model.Code,
                CourseName = model.Course.Name,
                MaxStudents = model.MaxStudents,
                EndDate = model.EndDate.ToVnTime(),
                StartDate = model.StartDate.ToVnTime(),
                TeacherName = string.Join(" ", model.Staff.FirstName, model.Staff.LastName),
                Status = model.Status,
                StudentsInClass = null,
                StudentsAddToClass = null,
                ClassId = model.Id,
                TeacherId = model.Staff.Id,
                CourseId = model.CourseId,
                Note = model.Note,
                StudentCount = currentStudents,
            };
            return data;
        }

        public async Task SoftDeleteAsync(long id)
        {
            var model = await _context.Classes
                .Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            model.IsDeleted = true;
            model.UpdatedBy = _currentUser.FullName;
            model.UpdatedDate = DateTimeOffset.UtcNow.ToUtcDb();
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CreatedClassDto updateClass)
        {
            var model = await _context.Classes
                .Where(x => x.Id == updateClass.ClassId && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if (model == null)
                throw new Exception("Class not found");
            var courseModel = await _context.Courses
                .Where(x => x.Id == (long)updateClass.CourseId)
                .FirstOrDefaultAsync();
            int DurationInMonths = courseModel?.DurationInMonths ?? 0;
            model.Note = updateClass.Note;
            model.StartDate = updateClass.StartDate.Value.ToUtcDb();
            model.EndDate = updateClass.StartDate.Value.AddMonths(DurationInMonths).ToUtcDb();
            model.Status = updateClass.Status;
            model.UpdatedBy = _currentUser.FullName;
            model.UpdatedDate = DateTimeOffset.UtcNow.ToUtcDb();
            model.Code = updateClass.ClassCode;
            model.CourseId = updateClass.CourseId;
            model.StaffId = updateClass.TeacherId;
            model.MaxStudents = updateClass.MaxStudents;
            await _context.SaveChangesAsync();

        }
    }
}
