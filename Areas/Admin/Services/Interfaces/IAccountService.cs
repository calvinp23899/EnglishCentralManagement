using EnglishCentralManagement.Dtos;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IAccountService
    {
        Task<CreatedStaffDto?> GetByStaffIdAsync(long id);
        Task<CreatedAccountDto?> CreateAccount(CreatedAccountDto model);
        Task UpdatedAccount(UpdatedAccountDto model);

        Task<List<TeacherSelectDto>> GetListTeacherSelectedAsync();
        Task<ProfileDto> GetProfileAsync(long userId);
        Task ChangePassword(long userId, string newPassword);

    }
}
