using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IClassSessionService
    {
        Task<PagedResult<SessionDto>> GetAllClassSession(long classId, int pageIndex, int pageSize, string search);
        Task CreateSessionClass(CreateSessionDto createSession);
        Task<SessionDto> GetSessionDetail(long sessionId);
        Task UpdateSessionDetail(UpdateSessionDto updateSession);
        Task DeleteSession(long id);
        Task<List<StudentAttendance>> GetStudentsBySession(long id, long classId);
        Task SaveAttendance(long sessionId, List<StudentAttendanceRequest> model);
    }
}
