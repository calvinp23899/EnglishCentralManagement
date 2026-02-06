using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class AuthService : IAuthService
    {
        private readonly EnglishCentreDbContext _context;

        public AuthService(EnglishCentreDbContext context)
        {
            _context = context;
        }
        public  async Task<Account?> Login(string username, string password)
        {
            var account =  _context.Accounts
                .Include(x => x.Role)
            .FirstOrDefault(x => x.Username == username && x.IsDeleted == false);

            if (account == null)
                return null;

            if (!EncryptHelper.Verify(password, account.PasswordHash))
                return null;

            return account;
        }
    }
}
