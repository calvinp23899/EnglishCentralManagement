using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models;
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

        public async Task CreateAsync(CreatedClassDto newClass)
        {
            var data = new Class
            {
                Code = newClass.ClassCode,
                StartDate = newClass.StartDate.Value.ToUniversalTime(),
                EndDate = newClass.EndDate.Value.ToUniversalTime(),
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
                .Include(x=>x.Course)
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
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
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

        public Task<PagedResult<ClassDto>> GetAllMyClassAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<CreatedClassDto> UpdateAsync(CreatedClassDto updateClass)
        {
            throw new NotImplementedException();
        }
    }
}
