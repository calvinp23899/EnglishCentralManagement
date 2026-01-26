using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class LoginController : AdminBaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {        
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            string errorMsg = null;
            if (username == "admin" && password == "123")
            {


                return RedirectToAction(
                    "Index",
                    "Home",
                    new { area = "Admin" }
                );
            }
            errorMsg = "Sai tài khoản hoặc mật khẩu";
            ViewBag.Error = errorMsg;
            return View();
        }
    }
}
