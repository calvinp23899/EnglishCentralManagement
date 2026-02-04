using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models;
using EnglishCentralManagement.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class StudentService : IStudentService
    {
        private readonly EnglishCentreDbContext _context;

        public StudentService(EnglishCentreDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreatedStudentDto newStudent)
        {
            var data = new Student
            {
                FirstName =  newStudent.FirstName,
                LastName =  newStudent.LastName,
                DateOfBirth =  newStudent.DateOfBirth,
                PhoneNumber =  newStudent.PhoneNumber,
                Email =  newStudent.Email,
                Address =  newStudent.Address,
                Gender =  newStudent.Gender > 0 ? true : false,
                Status = newStudent.Status.Value,
                CreatedBy = "Admin",
                CreatedDate = DateTimeOffset.UtcNow,
            };
            var account = new Account
            {
                Username = newStudent.Username,
                PasswordHash = EncryptHelper.Hash(newStudent.Password),
                RoleId = (long?)RoleType.User,
                CreatedBy = "Admin",
                CreatedDate = DateTimeOffset.UtcNow,
                Student = data,
            };
            _context.Students.Add(data);
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<StudentDto>> GetAllAsync(int pageIndex, int pageSize)
        {
            var query = _context.Students
                .Where(x => !x.IsDeleted);

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
                .Include(x=>x.Student)
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
                Password = model.PasswordHash,
                Role = (RoleType)model.RoleId,
                StudentId =  model.StudentId,
            };
            return data;
        }

        public async Task SoftDeleteAsync(long id)
        {
            var model = await _context.Students
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            model.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<CreatedStudentDto> UpdateAsync(CreatedStudentDto updatedStudent)
        {
            var model = await _context.Students
                .FirstOrDefaultAsync(x => x.Id == updatedStudent.StudentId && !x.IsDeleted);

            if (model == null)
                throw new Exception("Teacher not found");

            model.FirstName = updatedStudent.FirstName;
            model.LastName = updatedStudent.LastName;
            model.DateOfBirth = updatedStudent.DateOfBirth;
            model.PhoneNumber = updatedStudent.PhoneNumber;
            model.Email = updatedStudent.Email;
            model.Address = updatedStudent.Address;
            model.Gender = updatedStudent.Gender.Value > 0 ? true : false;
            model.Status = (StudentStatus)updatedStudent.Status;
            model.UpdatedDate = DateTimeOffset.UtcNow;
            model.UpdatedBy = "Admin";
            await _context.SaveChangesAsync();
            return updatedStudent;
        }
    }
}
