using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class AuthService : IAuthService
    {
        private readonly EnglishCentreDbContext _context;

        public AuthService(EnglishCentreDbContext context)
        {
            _context = context;
        }
        public async Task<AccountDto?> Login(string username, string password)
        {
            var account = _context.Accounts
                .Include(x => x.Role)
                .Include(x => x.Staff)
            .FirstOrDefault(x => x.Username == username && x.IsDeleted == false);

            if (account == null)
                return null;

            if (!EncryptHelper.Verify(password, account.PasswordHash))
                return null;
            var data = new AccountDto
            {
                Id = account.Id,
                StaffId = account.Staff.Id,
                FirstName = account.Staff.FirstName,
                LastName = account.Staff.LastName,
                Role = Common.GetDisplayEnumName((RoleType)account.RoleId),
            };
            return data;
        }
    }
}
