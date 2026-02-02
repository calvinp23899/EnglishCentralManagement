using EnglishCentralManagement.Models.Enum;
using EnglishCentralManagement.Models;
using Microsoft.EntityFrameworkCore;
using EnglishCentralManagement.Helpers;


namespace EnglishCentralManagement.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(EnglishCentreDbContext context)
        {
            // Đảm bảo DB đã tồn tại
            await context.Database.MigrateAsync();

            await SeedRoles(context);
            await SeedCourses(context);
            await SeedStaffs(context);
            await SeedClasses(context);
            await SeedStudents(context);
            await SeedEnrollments(context);
            await SeedAccounts(context);
        }

        // -------------------------
        private static async Task SeedCourses(EnglishCentreDbContext context)
        {
            if (context.Courses.Any()) return;

            context.Courses.AddRange(
                new Course
                {
                    Name = "IELTS Foundation",
                    DurationInMonths = 6,
                    MonthlyFee = 2500000
                },
                new Course
                {
                    Name = "IELTS Intermediate",
                    DurationInMonths = 6,
                    MonthlyFee = 3000000
                },
                new Course
                {
                    Name = "IELTS Advanced",
                    DurationInMonths = 9,
                    MonthlyFee = 3500000
                }
            );

            await context.SaveChangesAsync();
        }

        // -------------------------
        private static async Task SeedStaffs(EnglishCentreDbContext context)
        {
            if (context.Staffs.Any()) return;

            context.Staffs.AddRange(
                new Staff
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john@englishcentre.com",
                    PhoneNumber = "0909000001",
                    Title = "Senior Teacher",
                    Status = TeacherStatus.Active,
                    WorkingType = WorkingType.Onsite,
                    ContractType = ContractType.FullTime,
                    YearsOfExperience = 5,
                    MonthlySalary = 20000000,
                    CreatedBy = "System",
                    DateOfBirth = DateTime.Now,
                    OnboardingDate = DateTimeOffset.UtcNow,
                    Gender = true,
                    CreatedDate = DateTimeOffset.UtcNow,
                },
                new Staff
                {
                    FirstName = "Anna",
                    LastName = "Lee",
                    Email = "anna@englishcentre.com",
                    PhoneNumber = "0909000002",
                    Title = "Senior Teacher",
                    Status = TeacherStatus.Active,
                    WorkingType = WorkingType.Hybrid,
                    ContractType = ContractType.PartTime,
                    HourlyRate = 350000,
                    CreatedBy = "System",
                    DateOfBirth = DateTime.Now,
                    OnboardingDate = DateTimeOffset.UtcNow,
                    Gender = false,                   
                    CreatedDate = DateTimeOffset.UtcNow,
                }
            );

            await context.SaveChangesAsync();
        }

        // -------------------------
        private static async Task SeedClasses(EnglishCentreDbContext context)
        {
            if (context.Classes.Any()) return;

            var course = context.Courses.First();
            var teacher = context.Staffs.First();

            context.Classes.Add(new Class
            {
                Code = "IELTS-F-01",
                CourseId = course.Id,
                StaffId = teacher.Id,
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTimeOffset.UtcNow.AddMonths(course.DurationInMonths),
                MaxStudents = 20
            });

            await context.SaveChangesAsync();
        }

        // -------------------------
        private static async Task SeedStudents(EnglishCentreDbContext context)
        {
            if (context.Students.Any()) return;

            context.Students.Add(
                new Student
                {
                    FirstName = "Nguyen",
                    LastName = "An",
                    Email = "an@student.com",
                    PhoneNumber = "0911000001",
                    Status = StudentStatus.Active,
                    DateOfBirth = DateTime.Now,
                    Gender = false,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow,
                }
            );

            await context.SaveChangesAsync();
        }

        // -------------------------
        private static async Task SeedEnrollments(EnglishCentreDbContext context)
        {
            if (context.Enrollments.Any()) return;

            var student = context.Students.First();
            var cls = context.Classes.First();

            context.Enrollments.Add(new Enrollment
            {
                StudentId = student.Id,
                ClassId = cls.Id,
                EnrolledAt = DateTimeOffset.UtcNow,
                StartDate = cls.StartDate,
                EndDate = cls.EndDate,
                Status = EnrollmentStatus.Active
            });

            await context.SaveChangesAsync();
        }

        // -------------------------
        private static async Task SeedAccounts(EnglishCentreDbContext context)
        {
            if (context.Accounts.Any()) return;

            context.Accounts.AddRange(
                new Account{
                    Username = "admin",
                    PasswordHash = EncryptHelper.Hash("123456"), 
                    RoleId = (int)RoleType.Admin,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow,
                    StaffId = 2
                },
                new Account
                {
                    Username = "student1",
                    PasswordHash = EncryptHelper.Hash("123456"),
                    RoleId = (int)RoleType.User,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow,
                    StudentId = 1
                },
                new Account
                {
                    Username = "teacher1",
                    PasswordHash = EncryptHelper.Hash("123456"),
                    RoleId = (int)RoleType.Teacher,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow,
                    StaffId = 1
                });

            await context.SaveChangesAsync();
        }
        private static async Task SeedRoles(EnglishCentreDbContext context)
        {
            if (context.Roles.Any()) return;

            context.Roles.AddRange(
            new Role
            {
                Name = RoleType.User.GetDisplayEnumName(),
                CreatedBy = "System",
                CreatedDate = DateTimeOffset.UtcNow,
            },
            new Role
            {
                Name = RoleType.Admin.GetDisplayEnumName(),
                CreatedBy = "System",
                CreatedDate = DateTimeOffset.UtcNow,
            },
            new Role
            {
                Name = RoleType.Teacher.GetDisplayEnumName(),
                CreatedBy = "System",
                CreatedDate = DateTimeOffset.UtcNow,
            },
            new Role
            {
                Name = RoleType.Manager.GetDisplayEnumName(),
                CreatedBy = "System",
                CreatedDate = DateTimeOffset.UtcNow,
            },
            new Role
            {
                Name = RoleType.HR.GetDisplayEnumName(),
                CreatedBy = "System",
                CreatedDate = DateTimeOffset.UtcNow,
            },
            new Role
            {
                Name = RoleType.Coordinator.GetDisplayEnumName(),
                CreatedBy = "System",
                CreatedDate = DateTimeOffset.UtcNow,
            });

            await context.SaveChangesAsync();
        }
    }

}
