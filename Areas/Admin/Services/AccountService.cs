using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models;
using EnglishCentralManagement.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class AccountService : IAccountService
    {
        private readonly EnglishCentreDbContext _context;

        public AccountService(EnglishCentreDbContext context)
        {
            _context = context;
        }

        public async Task ChangePassword(long userId, string newPassword)
        {
            var model = await _context.Accounts
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();
            model.PasswordHash = EncryptHelper.Hash(newPassword);
            await _context.SaveChangesAsync();
        }

        public async Task<CreatedAccountDto?> CreateAccount(CreatedAccountDto model)
        {
            var account = new Account
            {
                Username = model.Username,
                PasswordHash = EncryptHelper.Hash(model.Password),
                RoleId = (long?)model.RoleId,
                CreatedBy = "Admin",
                CreatedDate = DateTimeOffset.UtcNow,
                StaffId = model.StaffId ?? null,
                StudentId = model.StudentId ?? null,
            };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<CreatedStaffDto?> GetByStaffIdAsync(long id)
        {
            var model = await _context.Accounts.Include(x => x.Staff)
                .FirstOrDefaultAsync(x => x.StaffId == id && !x.IsDeleted);
            var data = new CreatedStaffDto
            {
                FirstName = model.Staff.FirstName,
                LastName = model.Staff.LastName,
                DateOfBirth = (DateTime)model.Staff.DateOfBirth,
                PhoneNumber = model.Staff.PhoneNumber,
                Email = model.Staff.Email,
                ContractType = model.Staff.ContractType,
                WorkingType = model.Staff.WorkingType,
                YearsOfExperience = model.Staff.YearsOfExperience,
                HourlyRate = model.Staff.HourlyRate,
                MonthlySalary = model.Staff.MonthlySalary,
                OnboardingDate = model.Staff.OnboardingDate.Value.ToUniversalTime(),
                Address = model.Staff.Address,
                Gender = model.Staff.Gender == true ? GenderEnum.Male : GenderEnum.Female,
                Title = model.Staff.Title,
                StaffId = model.StaffId,
                Username = model.Username,
                Password = model.PasswordHash,
                Role = (RoleType?)model.RoleId,
            };
            return data;
        }

        public async Task<List<TeacherSelectDto>> GetListTeacherSelectedAsync()
        {
            var data = new List<TeacherSelectDto>();
            data = await _context.Accounts
                .Include(x => x.Staff)
                .Where(x => x.RoleId == (long)RoleType.Teacher && !x.IsDeleted)
                .Select(x => new TeacherSelectDto
                {
                    Id = (long)x.StaffId,
                    FullName = string.Concat(x.Staff.FirstName + " " + x.Staff.LastName).Trim()
                })
                .ToListAsync();
            return data;
        }

        public async Task<ProfileDto> GetProfileAsync(long userId)
        {
            var data = await _context.Accounts
                .Include(x => x.Staff)
                .Where(x => x.Id == userId && !x.IsDeleted)
                .Select(x => new ProfileDto
                {
                    FullName = string.Concat(x.Staff.FirstName + " " + x.Staff.LastName).Trim(),
                    Address = x.Staff.Address,
                    PhoneNumber = x.Staff.PhoneNumber,
                    Email = x.Staff.Email,
                    Gender = x.Staff.Gender == true ? "Male" : "Female",
                    DateOfBirth = (DateTimeOffset)x.Staff.DateOfBirth,
                    Role = Common.GetDisplayEnumName((RoleType)x.RoleId),
                    Title = x.Staff.Title,
                    ContractType = x.Staff.ContractType.GetDisplayEnumName(),
                })
                .FirstOrDefaultAsync();
            return data;
        }

        public async Task UpdatedAccount(UpdatedAccountDto model)
        {
            var data = await _context.Accounts
                .FirstOrDefaultAsync(x => x.Id == model.AccountId && !x.IsDeleted);
            if (data == null)
            {
                return;
            }
            data.PasswordHash = EncryptHelper.Hash(model.Password);
            data.RoleId = (long)model.RoleType;
            await _context.SaveChangesAsync();
        }
    }
}
