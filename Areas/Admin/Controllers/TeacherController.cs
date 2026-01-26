using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class TeacherController : AdminBaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
