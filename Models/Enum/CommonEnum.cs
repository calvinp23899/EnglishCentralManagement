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

    public enum StudentStatus { 
        Active, 
        Inactive 
    }
    public enum EnrollmentStatus { 
        Active, 
        Completed, 
        Cancelled 
    }
    public enum PaymentScheduleStatus { 
        Pending, 
        Paid, 
        Overdue, 
        Cancelled 
    }
    public enum PaymentMethod { 
        Cash, 
        BankTransfer,
    }
    public enum RoleType
    {
        User, //0
        Admin, //1
        HR, //2
        Manager, //3
        Accountant, //4
        Teacher, // 5
        Coordinator // 6
    }

    public enum GenderEnum
    {
        Female = 0,
        Male = 1,        
    }
}
