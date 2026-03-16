using System.ComponentModel.DataAnnotations;

namespace EnglishCentralManagement.Models.Enum
{
    public enum TeacherStatus
    {
        [Display(Name = "Active")]
        Active = 1,
        [Display(Name = "Inactive")]
        Inactive = 2,
        [Display(Name = "OnLeave")]
        OnLeave = 3
    }

    public enum ContractType
    {
        FullTime = 1,
        PartTime = 2,
        Freelancer = 3
    }

    public enum WorkingType
    {
        Onsite = 1,
        Online = 2,
        Hybrid = 3
    }

    public enum StudentStatus
    {
        [Display(Name = "Active")]
        Active = 1,
        [Display(Name = "Inactive")]
        Inactive = 0
    }
    public enum EnrollmentStatus
    {
        [Display(Name = "Active")]
        Active = 1,
        [Display(Name = "InActive")]
        InActive = 2,
        [Display(Name = "Completed")]
        Completed = 3,
        [Display(Name = "Cancelled")]
        Cancelled = 4
    }
    public enum PaymentScheduleStatus
    {
        Pending,
        Paid,
        Overdue,
        Cancelled
    }
    public enum PaymentMethod
    {
        Cash,
        BankTransfer,
    }
    public enum RoleType
    {
        [Display(Name = "User")]
        User = 1,
        [Display(Name = "Admin")]
        Admin = 2,
        [Display(Name = "HR")]
        HR = 3,
        [Display(Name = "Manager")]
        Manager = 4,
        [Display(Name = "Accountant")]
        Accountant = 5,
        [Display(Name = "Teacher")]
        Teacher = 6,
        [Display(Name = "Coordinator")]
        Coordinator = 7
    }

    public enum GenderEnum
    {
        Female = 0,
        Male = 1,
    }

    public enum ClassStatusEnum
    {
        [Display(Name = "Active")]
        Active = 1,
        [Display(Name = "Completed")]
        Completed = 2,
        [Display(Name = "Cancelled")]
        Cancelled = 3,
    }

    public enum ExpenseCategory
    {
        [Display(Name = "TeacherSalary")]
        TeacherSalary = 1,
        [Display(Name = "AdminSalary")]
        AdminSalary = 2,
        [Display(Name = "Rent")]
        Rent = 3, //House Rent
        [Display(Name = "Utilities")]
        Utilities = 4, //Water, Electricbill, Internet, ...
        [Display(Name = "Marketing")]
        Marketing = 5,
        [Display(Name = "Material")]
        Material = 6, //Class Material
        [Display(Name = "Commission")]
        Commission = 7,
        [Display(Name = "Other")]
        Other = 8,
        [Display(Name = "HRSalary")]
        HRSalary = 9,
        [Display(Name = "AccountantSalary")]
        AccountantSalary = 10,
        [Display(Name = "ManagerSalary")]
        ManagerSalary = 11

    }

    public enum ExpenseType
    {
        //Để tính break-even
        Fixed = 1,
        Variable = 2
    }

    public enum SessionStatusEnum
    {
        [Display(Name = "OnGoing")]
        OnGoing = 0,    // Vẫn đang 
        [Display(Name = "Completed")]
        Completed = 1,    // Đã dạy xong
        [Display(Name = "Cancelled")]
        Cancelled = 2,    // Huỷ buổi
        [Display(Name = "Rescheduled")]
        Rescheduled = 3,   // Dời lịch
        [Display(Name = "Scheduled")]
        Scheduled = 4   // Tạo trước
    }

    public enum AttendanceStatusEnum
    {
        [Display(Name = "Present")]
        Present = 1,
        [Display(Name = "Absent")]
        Absent = 2,
    }

    public enum IconEnum
    {
        [Display(Name = "Facebook")]
        Facebook = 1,
        [Display(Name = "Instagram")]
        Instagram = 2,
    }

    public enum FooterTypeEnum
    {
        [Display(Name = "Branch")]
        Branch = 1,
        [Display(Name = "Course")]
        Course = 2,
        [Display(Name = "Contact")]
        Contact = 3,
    }

    public enum FolderCloudFare
    {
        [Display(Name = "User")]
        User = 1,
        [Display(Name = "Admin")]
        Admin = 2,
    }
}
