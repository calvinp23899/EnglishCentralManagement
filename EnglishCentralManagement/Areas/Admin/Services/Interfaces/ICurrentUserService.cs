namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface ICurrentUserService
    {
        long? UserId { get; }
        string? FullName { get; }
        string? UserRole { get; }
        long? StaffId { get; }

    }
}
