using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Extensions;
using EnglishCentralManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class CourseService : ICourseService
    {
        private readonly EnglishCentreDbContext _context;
        private readonly ICurrentUserService _currentUser;


        public CourseService(EnglishCentreDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task CreateCourseAsync(CourseDto newCourse)
        {
            var data = new Course
            {
                Name = newCourse.CourseName,
                MonthlyFee = (decimal)newCourse.MonthlyFee,
                DurationInMonths = newCourse.DurationInMonth,
                CreatedBy = _currentUser.FullName,
                CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
            };
            _context.Courses.Add(data);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourseAsync(long id)
        {
            var model = await _context.Courses
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            model.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<CourseDto>> GetAllCourseAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.Courses
                .Where(x => !x.IsDeleted);
            if (!string.IsNullOrEmpty(search))
            {
                string searchLower = search.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(searchLower)
               );
            }
            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new CourseDto
                {
                    Id = x.Id,
                    CourseName = x.Name,
                    DurationInMonth = x.DurationInMonths,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    MonthlyFee = x.MonthlyFee,
                })
                .ToListAsync();
            return new PagedResult<CourseDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<List<CourseSelectDto>> GetCourseSelectedAsync()
        {
            var data = await _context.Courses.Where(x => x.IsDeleted == false)
                .Select(x => new CourseSelectDto
                {
                    Id = x.Id,
                    CourseName = x.Name
                })
                .ToListAsync();
            return data;
        }

        public async Task UpdateCourseAsync(CourseDto updateCourse)
        {
            var model = await _context.Courses
                .Where(x => !x.IsDeleted && x.Id == updateCourse.Id)
                .FirstOrDefaultAsync();
            model.Name = updateCourse.CourseName;
            model.MonthlyFee = (decimal)updateCourse.MonthlyFee;
            model.DurationInMonths = updateCourse.DurationInMonth;
            model.UpdatedDate = DateTimeOffset.UtcNow.ToUtcDb();
            model.UpdatedBy = _currentUser.FullName;
            await _context.SaveChangesAsync();
        }

        public async Task<CourseDto> ViewCourseDetailAsync(long id)
        {
            var model = await _context.Courses
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            var data = new CourseDto
            {
                Id = model.Id,
                CourseName = model.Name,
                DurationInMonth = model.DurationInMonths,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
                MonthlyFee = model.MonthlyFee,
            };
            return data;
        }
    }
}
