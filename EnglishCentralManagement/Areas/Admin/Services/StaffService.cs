using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Extensions;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models;
using EnglishCentralManagement.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class StaffService : IStaffService
    {
        private readonly EnglishCentreDbContext _context;
        private readonly ICurrentUserService _currentUser;


        public StaffService(EnglishCentreDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<PagedResult<StaffDto>> GetAllAsync(int pageIndex, int pageSize, string? search = null)
        {
            var query = _context.Staffs
                .Where(x => !x.IsDeleted);
            if (!string.IsNullOrEmpty(search))
            {
                string searchLower = search.ToLower();
                query = query.Where(x =>
                    x.FirstName.ToLower().Contains(searchLower) ||
                    x.LastName.ToLower().Contains(searchLower) ||
                    x.Email.ToLower().Contains(searchLower) ||
                    x.PhoneNumber.Contains(searchLower)
               );
            }

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
                    Status = Common.GetDisplayEnumName(x.Status)
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
                WorkingHour = model.WorkingHour,
                Status = model.Status.ToString(),
                YearsOfExperience = model.YearsOfExperience,
                HourlyRate = model.HourlyRate,
                OnboardingDate = model.OnboardingDate,
                Bank = model.BankCard,
                CardNumber = model.PaymentCard,
                Image = model.AvatarUrl,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
                UpdatedBy = model.UpdatedBy,
                UpdatedDate = model.UpdatedDate
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
                WorkingHour = newStaff.WorkingHour,
                PaymentCard = newStaff.PaymentCard,
                BankCard = newStaff.BankCard,
                MonthlySalary = newStaff.HourlyRate * newStaff.WorkingHour,
                OnboardingDate = newStaff.OnboardingDate.HasValue
                                    ? newStaff.OnboardingDate.Value.ToUtcDb()
                                    : null,
                Address = newStaff.Address,
                Gender = newStaff.Gender.Value > 0 ? true : false,
                CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                CreatedBy = _currentUser.FullName,
                AvatarUrl = "/assets/img/avatar-default.png",
                Title = newStaff.Title,
            };
            var isAccountExist = await _context.Accounts
                 .AnyAsync(x => x.Username.ToLower() == newStaff.Username.ToLower() && !x.IsDeleted);
            if (isAccountExist)
                throw new Exception("Username is already existed");
            var account = new Account
            {
                Username = newStaff.Username.ToLower(),
                PasswordHash = EncryptHelper.Hash(newStaff.Password),
                RoleId = (long?)newStaff.Role,
                CreatedBy = _currentUser.FullName,
                CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                Staff = staff,
            };
            _context.Staffs.Add(staff);
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task<UpdatedStaffDto> UpdateAsync(UpdatedStaffDto updateStaff)
        {
            var model = await _context.Staffs
                .FirstOrDefaultAsync(x => x.Id == updateStaff.StaffId && !x.IsDeleted);

            if (model == null)
                throw new Exception("Student not found");

            var accountModel = await _context.Accounts
                .FirstOrDefaultAsync(x => x.StaffId == updateStaff.StaffId && !x.IsDeleted);

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
            model.WorkingHour = updateStaff.WorkingHour;
            model.BankCard = updateStaff.BankCard;
            model.PaymentCard = updateStaff.PaymentCard;
            model.MonthlySalary = updateStaff.HourlyRate * updateStaff.WorkingHour;
            model.OnboardingDate = updateStaff.OnboardingDate.HasValue
                                    ? updateStaff.OnboardingDate.Value.ToUtcDb()
                                    : null;
            model.Title = updateStaff.Title;
            model.UpdatedDate = DateTimeOffset.UtcNow.ToUtcDb();
            model.UpdatedBy = _currentUser.FullName;
            //Change Password
            if (!string.IsNullOrEmpty(updateStaff.Password))
            {
                accountModel.PasswordHash = EncryptHelper.Hash(updateStaff.Password);
            }
            accountModel.RoleId = (long?)updateStaff.Role;
            await _context.SaveChangesAsync();
            return updateStaff;
        }

        public async Task SoftDeleteAsync(long id)
        {
            var staff = await _context.Staffs.FindAsync(id);

            if (staff == null)
                throw new Exception("Teacher not found");

            staff.IsDeleted = true;
            staff.UpdatedDate = DateTimeOffset.UtcNow.ToUtcDb();
            staff.UpdatedBy = _currentUser.FullName;

            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAllStaff()
        {
            int data = await _context.Staffs.CountAsync();
            return data;
        }

        public async Task<List<StaffExcelDto>> GetStaffForExcel()
        {
            var listModel = await _context.Staffs
                .Select(x => new StaffExcelDto
                {
                    FullName = string.Concat(x.FirstName + " " + x.LastName).Trim(),
                    Email = x.Email,
                    ContractType = x.ContractType.GetDisplayEnumName(),
                    BankCard = x.BankCard,
                    PaymentCard = x.PaymentCard,
                    HourlyRate = x.HourlyRate,
                    WorkingHour = x.WorkingHour,
                    MonthlySalary = x.MonthlySalary
                })
                .ToListAsync();
            return listModel;
        }
    }
}
