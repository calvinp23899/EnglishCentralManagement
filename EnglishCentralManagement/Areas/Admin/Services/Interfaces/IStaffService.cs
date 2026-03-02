using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IStaffService
    {
        Task<PagedResult<StaffDto>> GetAllAsync(int pageIndex, int pageSize, string search);
        Task<StaffDetailDto?> GetByIdAsync(long id);
        Task CreateAsync(CreatedStaffDto staff);
        Task<UpdatedStaffDto> UpdateAsync(UpdatedStaffDto staff);
        Task SoftDeleteAsync(long id);
        Task<int> CountAllStaff();
        Task<List<StaffExcelDto>> GetStaffForExcel();

    }
}
