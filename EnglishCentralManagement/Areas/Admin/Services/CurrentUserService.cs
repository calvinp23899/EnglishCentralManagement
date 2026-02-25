using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using System.Security.Claims;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long? UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                return userId != null ? long.Parse(userId) : null;
            }
        }

        public string? FullName
        {
            get
            {
                var userName = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirstValue(ClaimTypes.Name);

                return userName != null ? userName : null;
            }
        }

        public string? UserRole
        {
            get
            {
                var userRole = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirstValue(ClaimTypes.Role);

                return userRole != null ? userRole : null;
            }
        }
    }
}
