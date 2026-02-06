using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using Microsoft.EntityFrameworkCore;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class CourseService : ICourseService
    {
        private readonly EnglishCentreDbContext _context;

        public CourseService(EnglishCentreDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseSelectDto>> GetCourseSelectedAsync()
        {
            var data = await _context.Courses.Where(x => x.IsDeleted == false)
                .Select(x=> new CourseSelectDto
                {
                    Id = x.Id,
                    CourseName = x.Name
                })
                .ToListAsync();
            return data;
        }
    }
}
