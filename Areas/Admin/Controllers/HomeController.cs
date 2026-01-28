using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
