using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "NotUser")]
    public abstract class AdminBaseController : Controller
    {
        protected AdminBaseController()
        {
        }

        protected long CurrentUserId()
        {
            return long.Parse(User.FindFirstValue("StaffId")!);
        }
    }
}
