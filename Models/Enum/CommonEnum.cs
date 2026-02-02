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
        [Display(Name = "Active")]
        Active = 1,
        [Display(Name = "Inactive")]
        Inactive = 0
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
}
