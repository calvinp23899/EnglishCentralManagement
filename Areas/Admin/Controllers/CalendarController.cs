using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class CalendarController : AdminBaseController
    {
        public CalendarController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
