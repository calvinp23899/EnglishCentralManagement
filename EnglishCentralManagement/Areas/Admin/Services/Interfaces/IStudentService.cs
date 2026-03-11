using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Dtos.User;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IStudentService
    {
        Task<PagedResult<StudentDto>> GetAllAsync(int pageIndex, int pageSize, string search);
        Task<CreatedStudentDto?> GetByIdAsync(long id);
        Task CreateAsync(CreatedStudentDto newStudent);
        Task RegisterStudentAsync(UserRegisterDto newStudent);
        Task<UpdateStudentDto> UpdateAsync(UpdateStudentDto updateStudent);
        Task SoftDeleteAsync(long id);
        Task<PagedResult<StudentListDto>> GetStudentNotInClassAsync(int pageIndex, int pageSize, long classId, string search);
        Task<int> CountAllStudent();
    }
}
