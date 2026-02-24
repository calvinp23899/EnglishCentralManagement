using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Models;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AccountDto?> Login(string username, string password);
    }
}
