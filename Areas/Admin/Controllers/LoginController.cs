using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Models.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class LoginController : AdminBaseController
    {
        private readonly IAuthService _authService;
        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {        
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var account = _authService.Login(model.Username, model.Password);

            if (account == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View("Index");
            }

            if (Enum.IsDefined(typeof(RoleType), (int)account.RoleId))
            {
                var roleName = ((RoleType)account.RoleId).ToString();
                HttpContext.Session.SetString("USER", account.Username);
                HttpContext.Session.SetString("ROLE", roleName);
            }
            else
            {
                HttpContext.Session.SetString("ROLE", "Unknown");
            }
            return RedirectToAction(
                    "Index",
                    "Home",
                    new { area = "Admin" }
            );
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
    }
}
