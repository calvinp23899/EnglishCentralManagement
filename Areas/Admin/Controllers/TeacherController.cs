using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class TeacherController : AdminBaseController
    {
        public TeacherController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string username)
        {
            return null;
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(string username)
        {
            return null;
        }

        [HttpGet]
        public IActionResult GetDetailId()
        {
            return View();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            return null;
        }
    }
}
