using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Models.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    [AllowAnonymous]
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
            var account = await _authService.Login(model.Username, model.Password);

            if (account == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View("Index");
            }
            string roleName = account.Role?.Name;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.StaffId.ToString()),
                new Claim(ClaimTypes.Name, string.Join(" ",account.Staff?.FirstName,account.Staff?.LastName).Trim()),
                new Claim(ClaimTypes.Role, roleName), 
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );
            return RedirectToAction(
                    "Index",
                    "Home",
                    new { area = "Admin" }
            );
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            HttpContext.Session.Clear();
            return View("Index");
        }

        public IActionResult AccessDenied(string? returnUrl = null)     
        {
            ViewBag.Error = "Bạn không có quyền truy cập vào trang này";
            return View("Index");
        }
    }
}
