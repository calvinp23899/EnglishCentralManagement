using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IStudentService
    {
        Task<PagedResult<StudentDto>> GetAllAsync(int pageIndex, int pageSize);
        Task<CreatedStudentDto?> GetByIdAsync(long id);
        Task CreateAsync(CreatedStudentDto newStudent);
        Task<CreatedStudentDto> UpdateAsync(CreatedStudentDto updateStudent);
        Task SoftDeleteAsync(long id);
        Task<PagedResult<StudentListDto>> GetStudentNotInClassAsync(int pageIndex, int pageSize, long classId);
        Task<int> CountAllStudent();
    }
}
