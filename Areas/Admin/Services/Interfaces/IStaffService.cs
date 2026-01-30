using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Models;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IStaffService
    {
        Task<PagedResult<StaffDto>> GetAllAsync(int pageIndex, int pageSize);
        Task<StaffDetailDto?> GetByIdAsync(long id);
        Task CreateAsync(CreatedStaffDto staff);
        Task UpdateAsync(Staff staff);
        Task SoftDeleteAsync(long id);
    }
}
