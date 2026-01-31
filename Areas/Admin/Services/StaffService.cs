using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models;
using EnglishCentralManagement.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class StaffService : IStaffService
    {
        private readonly EnglishCentreDbContext _context;

        public StaffService(EnglishCentreDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<StaffDto>> GetAllAsync(int pageIndex, int pageSize)
        {
            var query = _context.Staffs
                .Where(x => !x.IsDeleted);

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new StaffDto
                {
                    Id = x.Id,
                    FullName = (x.FirstName + " " + x.LastName).Trim(),
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Gender = (bool)x.Gender ? "Male" : "Female",
                    Status = Common.GetDisplayEnumRoleName(x.Status)
                })
                .ToListAsync();
            return new PagedResult<StaffDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<StaffDetailDto?> GetByIdAsync(long id)
        {
            var model = await _context.Staffs
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            var data = new StaffDetailDto
            {
                FullName = string.Concat(model.FirstName + " " + model.LastName).Trim(),
                PhoneNumber = model.PhoneNumber,
                Email = model.Email, 
                Gender = model.Gender == true ? "Male" : "Female",
                Address = model.Address,
                Title = model.Title,
                DateOfBirth = model.DateOfBirth.Value,
                WorkingType = model.WorkingType,
                ContractType = model.ContractType,
                MonthlySalary = model.MonthlySalary,
                Status = model.Status.ToString(),
                YearsOfExperience = model.YearsOfExperience,
                HourlyRate = model.HourlyRate,
                OnboardingDate = model.OnboardingDate,
                Image = model.AvatarUrl
            };
            return data;
        }

        public async Task CreateAsync(CreatedStaffDto newStaff)
        {
            var staff = new Staff
            {
                FirstName = newStaff.FirstName,
                LastName = newStaff.LastName,
                DateOfBirth = newStaff.DateOfBirth,
                PhoneNumber = newStaff.PhoneNumber,
                Email = newStaff.Email,
                Status = TeacherStatus.Active,
                ContractType = newStaff.ContractType,
                WorkingType = newStaff.WorkingType,
                YearsOfExperience = newStaff.YearsOfExperience,
                HourlyRate = newStaff.HourlyRate,
                MonthlySalary = newStaff.MonthlySalary,
                OnboardingDate = newStaff.OnboardingDate.Value.ToUniversalTime(),
                Address = newStaff.Address,
                Gender = newStaff.Gender.Value > 0 ? true : false,
                CreatedDate = DateTimeOffset.UtcNow,
                CreatedBy = "Admin",  
                AvatarUrl = "/assets/img/avatar-default.png",
                Title = newStaff.Title,
            };


            var account = new Account
            {
                Username = newStaff.Username,
                PasswordHash = EncryptHelper.Hash(newStaff.Password),
                RoleId = (long?)newStaff.Role,
                CreatedBy = "Admin",
                CreatedDate = DateTimeOffset.UtcNow,
                Staff = staff,
            };
            _context.Staffs.Add(staff);
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task<CreatedStaffDto> UpdateAsync(CreatedStaffDto updateStaff)
        {
            var model = await _context.Staffs
                .FirstOrDefaultAsync(x => x.Id == updateStaff.StaffId && !x.IsDeleted);

            if (model == null)
                throw new Exception("Teacher not found");

            model.FirstName = updateStaff.FirstName;
            model.LastName = updateStaff.LastName;
            model.DateOfBirth = updateStaff.DateOfBirth;
            model.PhoneNumber = updateStaff.PhoneNumber;
            model.Email = updateStaff.Email;
            model.Address = updateStaff.Address;
            //model.AvatarUrl = updateStaff.Avatar;
            model.YearsOfExperience = updateStaff.YearsOfExperience;
            model.ContractType = updateStaff.ContractType;
            model.WorkingType = updateStaff.WorkingType;
            model.HourlyRate = updateStaff.HourlyRate;
            model.MonthlySalary = updateStaff.MonthlySalary;
            model.OnboardingDate = updateStaff.OnboardingDate.Value.ToUniversalTime();
            model.Title = updateStaff.Title;
            model.UpdatedDate = DateTimeOffset.UtcNow;
            model.UpdatedBy = "Admin";

            await _context.SaveChangesAsync();
            return updateStaff;
        }

        public async Task SoftDeleteAsync(long id)
        {
            var staff = await _context.Staffs.FindAsync(id);

            if (staff == null)
                throw new Exception("Teacher not found");

            staff.IsDeleted = true;
            staff.UpdatedDate = DateTimeOffset.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
