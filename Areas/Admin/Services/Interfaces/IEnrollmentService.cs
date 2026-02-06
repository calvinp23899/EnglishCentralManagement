using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Dtos;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<PagedResult<StudentListDto>> GetStudentInClassAsync(int pageIndex, int pageSize, long classId);
        Task AddStudentInClassAsync(long studentId, long classId);
    }
}
