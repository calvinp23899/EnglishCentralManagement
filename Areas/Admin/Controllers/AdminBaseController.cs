using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "NotUser")]
    public abstract class AdminBaseController : Controller
    {
    }
}
