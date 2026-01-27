using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class ProfileController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
