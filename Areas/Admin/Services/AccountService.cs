using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
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
    }
}
