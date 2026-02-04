using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Dtos;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseSelectDto>> GetCourseSelectedAsync();
    }
}
