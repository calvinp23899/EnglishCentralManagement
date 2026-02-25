using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IClassService
    {
        Task<PagedResult<ClassDto>> GetAllAsync(int pageIndex, int pageSize);
        Task<PagedResult<ClassDto>> GetAllMyClassAsync(int pageIndex, int pageSize, long teacherId);
        Task CreateAsync(CreatedClassDto newClass);
        Task UpdateAsync(CreatedClassDto updateClass);
        Task SoftDeleteAsync(long id);
        Task<int> CountClass();
        Task<ClassDetailDto> GetDetailById(long id);
        Task<CreatedClassDto> GetClassInfoForEdit(long classId, List<TeacherSelectDto> listTeacherSelect, List<CourseSelectDto> listCourseSelect);
    }
}
