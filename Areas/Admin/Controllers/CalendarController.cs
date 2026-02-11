using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class CalendarController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
