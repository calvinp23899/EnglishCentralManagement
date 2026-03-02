using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Invoice;
using EnglishCentralManagement.Dtos.Pagination;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<PagedResult<StudentListDto>> GetStudentInClassAsync(int pageIndex, int pageSize, long classId);
        Task<PagedResult<EnrollmentDto>> GetAllAsync(int pageIndex, int pageSize, string search);
        Task<EnrollmentDetailDto> GetDetailById(long id, int pageIndex, int pageSize);
        Task<TuitionReceiptDto> GetDetailInvoice(long enrollmentId);
    }
}
