using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseSelectDto>> GetCourseSelectedAsync();
        Task<PagedResult<CourseDto>> GetAllCourseAsync(int pageIndex, int pageSize, string search);
        Task CreateCourseAsync(CourseDto newCourse);
        Task UpdateCourseAsync(CourseDto updateCourse);
        Task DeleteCourseAsync(long id);
        Task<CourseDto> ViewCourseDetailAsync(long id);
    }
}
