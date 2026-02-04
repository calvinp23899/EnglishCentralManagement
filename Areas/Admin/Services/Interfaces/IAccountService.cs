using EnglishCentralManagement.Dtos;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IAccountService
    {
        Task<CreatedStaffDto?> GetByStaffIdAsync(long id);
        Task<CreatedAccountDto?> CreateAccount(CreatedAccountDto model);
        Task UpdatedAccount(UpdatedAccountDto model);

        Task<List<TeacherSelectDto>> GetListTeacherSelectedAsync();

    }
}
