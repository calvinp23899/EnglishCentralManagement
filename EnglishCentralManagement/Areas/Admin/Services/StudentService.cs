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
    public class StudentService : IStudentService
    {
        private readonly EnglishCentreDbContext _context;
        private readonly ICurrentUserService _currentUser;


        public StudentService(EnglishCentreDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<int> CountAllStudent()

        {
            var data = await _context.Students.CountAsync();
            if (data == null)
                data = 0;
            return data;
        }

        public async Task CreateAsync(CreatedStudentDto newStudent)
        {
            var data = new Student
            {
                FirstName = newStudent.FirstName,
                LastName = newStudent.LastName,
                DateOfBirth = newStudent.DateOfBirth,
                PhoneNumber = newStudent.PhoneNumber,
                Email = newStudent.Email,
                Address = newStudent.Address,
                Gender = newStudent.Gender > 0 ? true : false,
                Status = StudentStatus.Active,
                CreatedBy = _currentUser.FullName,
                CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
            };
            var isAccountExist = await _context.Accounts
                .AnyAsync(x => x.Username.ToLower() == newStudent.Username.ToLower() && !x.IsDeleted);
            if (isAccountExist)
                throw new Exception("Username is already existed");
            var account = new Account
            {
                Username = newStudent.Username.ToLower(),
                PasswordHash = EncryptHelper.Hash(newStudent.Password),
                RoleId = (long?)RoleType.User,
                CreatedBy = _currentUser.FullName,
                CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                Student = data,
            };
            _context.Students.Add(data);
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<StudentDto>> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            var query = _context.Students
                .Where(x => !x.IsDeleted);
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                    x.FirstName.ToLower().Contains(search.ToLower()) ||
                    x.LastName.ToLower().Contains(search.ToLower()) ||
                    x.Email.ToLower().Contains(search.ToLower()) ||
                    x.PhoneNumber.Contains(search)
               );
            }
            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new StudentDto
                {
                    Id = x.Id,
                    FullName = (x.FirstName + " " + x.LastName).Trim(),
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Gender = (bool)x.Gender ? "Male" : "Female",
                    Status = Common.GetDisplayEnumName(x.Status)
                })
                .ToListAsync();
            return new PagedResult<StudentDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<CreatedStudentDto?> GetByIdAsync(long id)
        {
            var model = await _context.Accounts
                .Include(x => x.Student)
                .FirstOrDefaultAsync(x => x.StudentId == id && !x.IsDeleted);
            var data = new CreatedStudentDto
            {
                FirstName = model.Student.FirstName,
                LastName = model.Student.LastName,
                DateOfBirth = (DateTime)model.Student.DateOfBirth,
                Address = model.Student.Address,
                Gender = model.Student.Gender == true ? GenderEnum.Male : GenderEnum.Female,
                Email = model.Student.Email,
                PhoneNumber = model.Student.PhoneNumber,
                Username = model.Username,
                Role = (RoleType)model.RoleId,
                Status = model.Student.Status,
                StudentId = model.StudentId,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
                UpdatedBy = model.UpdatedBy,
                UpdatedDate = model.UpdatedDate
            };
            return data;
        }

        public async Task<PagedResult<StudentListDto>> GetStudentNotInClassAsync(int pageIndex, int pageSize, long classId)
        {
            var query = _context.Students
                    .Where(s => !s.Enrollments.Any(e => e.ClassId == classId));

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(s => s.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new StudentListDto
                {
                    Id = s.Id,
                    FullName = string.Join(" ", s.FirstName, s.LastName),
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Status = s.Status.GetDisplayEnumName(),
                })
                .ToListAsync();

            return new PagedResult<StudentListDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task SoftDeleteAsync(long id)
        {
            var model = await _context.Students
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            model.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<UpdateStudentDto> UpdateAsync(UpdateStudentDto updatedStudent)
        {
            var model = await _context.Students
                .FirstOrDefaultAsync(x => x.Id == updatedStudent.StudentId && !x.IsDeleted);
            var account = await _context.Accounts
                .FirstOrDefaultAsync(x => x.StudentId == updatedStudent.StudentId && !x.IsDeleted);
            if (model == null)
                throw new Exception("Teacher is not found");

            model.FirstName = updatedStudent.FirstName;
            model.LastName = updatedStudent.LastName;
            model.DateOfBirth = updatedStudent.DateOfBirth;
            model.PhoneNumber = updatedStudent.PhoneNumber;
            model.Email = updatedStudent.Email;
            model.Address = updatedStudent.Address;
            model.Gender = updatedStudent.Gender.Value > 0 ? true : false;
            model.Status = (StudentStatus)updatedStudent.Status;
            model.UpdatedDate = DateTimeOffset.UtcNow.ToUtcDb();
            model.UpdatedBy = _currentUser.FullName;
            //Change Password
            if (!string.IsNullOrEmpty(updatedStudent.Password))
            {
                account.PasswordHash = EncryptHelper.Hash(updatedStudent.Password);
            }
            await _context.SaveChangesAsync();
            return updatedStudent;
        }
    }
}
