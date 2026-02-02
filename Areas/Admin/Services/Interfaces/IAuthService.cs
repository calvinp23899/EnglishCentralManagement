using EnglishCentralManagement.Models;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Account?> Login(string username, string password);
    }
}
