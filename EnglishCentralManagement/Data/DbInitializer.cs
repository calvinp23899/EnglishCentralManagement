using EnglishCentralManagement.Extensions;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models;
using EnglishCentralManagement.Models.Constants;
using EnglishCentralManagement.Models.Enum;
using Microsoft.EntityFrameworkCore;


namespace EnglishCentralManagement.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(EnglishCentreDbContext context)
        {
            // Auto create database and apply-migration, update-database
            await context.Database.MigrateAsync();

            await SeedRoles(context);
            await SeedCourses(context);
            await SeedStaffs(context);
            await SeedClasses(context);
            await SeedStudents(context);
            //await SeedEnrollments(context);
            await SeedAccounts(context);
            await SeedFooter(context);
            await SeedSection(context);
            await SeedSectionItems(context);
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
                    MonthlyFee = 2500000,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
                },
                new Course
                {
                    Name = "IELTS Intermediate",
                    DurationInMonths = 6,
                    MonthlyFee = 3000000,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
                },
                new Course
                {
                    Name = "IELTS Advanced",
                    DurationInMonths = 9,
                    MonthlyFee = 3500000,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
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
                    Address = "123test",
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
                    Address = "123test",
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
                StartDate = DateTimeOffset.UtcNow.ToUtcDb(),
                EndDate = DateTimeOffset.UtcNow.AddMonths(course.DurationInMonths),
                Status = ClassStatusEnum.Active,
                MaxStudents = 20,
                CreatedBy = "System",
                CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
            });

            await context.SaveChangesAsync();
        }

        // -------------------------
        private static async Task SeedStudents(EnglishCentreDbContext context)
        {
            if (context.Students.Any()) return;

            context.Students.AddRange(
                new Student
                {
                    FirstName = "Nguyen",
                    LastName = "An",
                    Email = "an@student.com",
                    PhoneNumber = "0911000001",
                    Status = StudentStatus.Active,
                    DateOfBirth = DateTime.Now,
                    Address = "123test",
                    Gender = false,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                },
                new Student
                {
                    FirstName = "Nguyen",
                    LastName = "B",
                    Email = "an@student.com",
                    PhoneNumber = "0911000001",
                    Status = StudentStatus.Active,
                    DateOfBirth = DateTime.Now,
                    Address = "123test",
                    Gender = false,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                },
                new Student
                {
                    FirstName = "Nguyen",
                    LastName = "C",
                    Email = "an@student.com",
                    PhoneNumber = "0911000001",
                    Status = StudentStatus.Active,
                    DateOfBirth = DateTime.Now,
                    Address = "123test",
                    Gender = false,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                },
                new Student
                {
                    FirstName = "Nguyen",
                    LastName = "D",
                    Email = "an@student.com",
                    PhoneNumber = "0911000001",
                    Status = StudentStatus.Active,
                    DateOfBirth = DateTime.Now,
                    Address = "123test",
                    Gender = false,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                },
                new Student
                {
                    FirstName = "Nguyen",
                    LastName = "E",
                    Email = "an@student.com",
                    PhoneNumber = "0911000001",
                    Status = StudentStatus.Active,
                    DateOfBirth = DateTime.Now,
                    Address = "123test",
                    Gender = false,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
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
                new Account
                {
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
                },
                new Account
                {
                    Username = "student2",
                    PasswordHash = EncryptHelper.Hash("123456"),
                    RoleId = (int)RoleType.User,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow,
                    StudentId = 2
                },
                new Account
                {
                    Username = "student3",
                    PasswordHash = EncryptHelper.Hash("123456"),
                    RoleId = (int)RoleType.User,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow,
                    StudentId = 3
                },
                new Account
                {
                    Username = "student4",
                    PasswordHash = EncryptHelper.Hash("123456"),
                    RoleId = (int)RoleType.User,
                    CreatedBy = "System",
                    CreatedDate = DateTimeOffset.UtcNow,
                    StudentId = 4
                },
                 new Account
                 {
                     Username = "student5",
                     PasswordHash = EncryptHelper.Hash("123456"),
                     RoleId = (int)RoleType.User,
                     CreatedBy = "System",
                     CreatedDate = DateTimeOffset.UtcNow,
                     StudentId = 5
                 }
            );

            await context.SaveChangesAsync();
        }
        private static async Task SeedRoles(EnglishCentreDbContext context)
        {
            if (context.Roles.Any()) return;

            var roles = Enum.GetValues(typeof(RoleType))
               .Cast<RoleType>()
               .Select(role => new Role
               {
                   Name = role.GetDisplayEnumName(),
                   CreatedBy = "System",
                   CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
               });

            context.Roles.AddRange(roles);

            await context.SaveChangesAsync();
        }
        private static async Task SeedFooter(EnglishCentreDbContext context)
        {
            if (context.FooterItems.Any()) return;

            context.FooterItems.AddRange(
                new FooterItem
                {
                    Description = "Zalo",
                    Order = 1,
                    Link = "#",
                    IsActive = true,
                    Icon = "Zalo",
                    IsContact = false,
                    IsBranch = false,
                    IsCourse = false,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                    CreatedBy = CommonConstant.BySystem
                },
                new FooterItem
                {
                    Description = "0900000000",
                    Order = 2,
                    Link = "#",
                    IsActive = true,
                    Icon = "Phone",
                    IsContact = false,
                    IsBranch = false,
                    IsCourse = false,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                    CreatedBy = CommonConstant.BySystem
                },
                new FooterItem
                {
                    Description = "Email: englishcentral@gmail.com",
                    Order = 1,
                    Link = "#",
                    IsActive = true,
                    Icon = "Gmail",
                    IsContact = false,
                    IsBranch = false,
                    IsCourse = false,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                    CreatedBy = CommonConstant.BySystem
                },
                new FooterItem
                {
                    Description = "Giờ hoạt động: 8:00 - 21:00",
                    Order = 2,
                    Link = "#",
                    IsActive = true,
                    Icon = "Clock",
                    IsContact = false,
                    IsBranch = false,
                    IsCourse = false,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                    CreatedBy = CommonConstant.BySystem
                },
                new FooterItem
                {
                    Description = "01 Ông Ích Khiêm, TP. Đà Nẵng",
                    Order = 1,
                    Link = "#",
                    IsActive = true,
                    Icon = "Location",
                    IsBranch = true,
                    IsContact = false,
                    IsCourse = false,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                    CreatedBy = CommonConstant.BySystem
                },
                new FooterItem
                {
                    Description = "Khoá IELTS Foundation",
                    Order = 1,
                    Link = "#",
                    IsActive = true,
                    Icon = "Location",
                    IsCourse = true,
                    IsBranch = false,
                    IsContact = false,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                    CreatedBy = CommonConstant.BySystem
                },
                new FooterItem
                {
                    Description = "Facebook",
                    Order = 1,
                    Link = "#",
                    IsActive = true,
                    Icon = "Facebook",
                    IsContact = true,
                    IsBranch = false,
                    IsCourse = false,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                    CreatedBy = CommonConstant.BySystem
                },
                new FooterItem
                {
                    Description = "IELTS Foundation",
                    Order = 1,
                    Link = "#",
                    Icon = "Facebook",
                    IsActive = true,
                    IsContact = false,
                    IsBranch = false,
                    IsCourse = true,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                    CreatedBy = CommonConstant.BySystem
                }
            );
            await context.SaveChangesAsync();
        }
        private static async Task SeedSection(EnglishCentreDbContext context)
        {
            if (context.HeaderBodyItems.Any()) return;
            context.HeaderBodyItems.AddRange(
                new HeaderBodySection
                {
                    Title = "khoá học",
                    NavTitle = "khoá học",
                    Description = "mô tả khoá học ở đây",
                    Link = NavTitleEnum.Course.GetDisplayEnumName(),
                    Order = 1,
                    IsNav = true,
                    IsSlider = false,
                    CreatedBy = CommonConstant.BySystem,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
                },
                new HeaderBodySection
                {
                    Title = "Giáo Viên",
                    NavTitle = "Giáo Viên",
                    Description = "mô tả giáo viên",
                    Link = NavTitleEnum.Teachers.GetDisplayEnumName(),
                    Order = 2,
                    IsNav = true,
                    IsSlider = false,
                    CreatedBy = CommonConstant.BySystem,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
                },
                new HeaderBodySection
                {
                    Title = "Chúng Tôi",
                    NavTitle = "Chúng Tôi",
                    Description = "mô tả chúng tôi",
                    Link = NavTitleEnum.AboutUs.GetDisplayEnumName(),
                    Order = 3,
                    IsNav = true,
                    IsSlider = false,
                    CreatedBy = CommonConstant.BySystem,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
                },
                new HeaderBodySection
                {
                    Link = "https://www.facebook.com/",
                    ImageUrl = "/assets/img/slider.png",
                    Order = 1,
                    IsNav = false,
                    IsSlider = true,
                    CreatedBy = CommonConstant.BySystem,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
                }
            );

            await context.SaveChangesAsync();
        }
        private static async Task SeedSectionItems(EnglishCentreDbContext context)
        {
            if (context.SectionItems.Any()) return;
            context.SectionItems.AddRange(
                new SectionItem
                {
                    Title = "Mrs. Linh",
                    Description = "IELTS 8.0",
                    ImageUrl = "/assets/img/avatar-default.png",
                    Order = 1,
                    HeaderBodySectionId = 2,
                    CreatedBy = CommonConstant.BySystem,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
                },
                new SectionItem
                {
                    Title = "Mrs. Ha",
                    Description = "IELTS 9.0",
                    ImageUrl = "/assets/img/avatar-default.png",
                    Order = 1,
                    HeaderBodySectionId = 2,
                    CreatedBy = CommonConstant.BySystem,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
                },
                new SectionItem
                {
                    Title = "Mr. Huy",
                    Description = "IELTS 8.0",
                    ImageUrl = "/assets/img/avatar-default.png",
                    Order = 1,
                    HeaderBodySectionId = 2,
                    CreatedBy = CommonConstant.BySystem,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
                },
                new SectionItem
                {
                    Title = "IELTS Beginner 3.0 - 5.0",
                    Description = "Đây là khoá học cho người đã có nền tảng tiếng anh cơ bản theo format ielts. Lộ trình bài bản được dạy bởi những thầy cô kinh nghiệm 7.0",
                    Order = 1,
                    HeaderBodySectionId = 1,
                    CreatedBy = CommonConstant.BySystem,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
                },
                new SectionItem
                {
                    Title = "IELTS Intermediate 5.0 - 6.5+",
                    Description = "Đây là khoá học cho người đã có nền tảng tiếng anh cơ bản theo format ielts. Lộ trình bài bản được dạy bởi những thầy cô kinh nghiệm 7.0",
                    Order = 2,
                    HeaderBodySectionId = 1,
                    CreatedBy = CommonConstant.BySystem,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
                },
                new SectionItem
                {
                    Title = "IELTS Intermediate  7.0+",
                    Description = "Đây là khoá học cho người đã có nền tảng tiếng anh cơ bản theo format ielts. Lộ trình bài bản được dạy bởi những thầy cô kinh nghiệm 7.0",
                    Order = 3,
                    HeaderBodySectionId = 1,
                    CreatedBy = CommonConstant.BySystem,
                    CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
                }
           );
            await context.SaveChangesAsync();
        }

    }

}
