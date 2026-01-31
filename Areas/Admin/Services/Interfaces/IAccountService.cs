using EnglishCentralManagement.Dtos;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IAccountService
    {
        Task<CreatedStaffDto?> GetByStaffIdAsync(long id);

    }
}
