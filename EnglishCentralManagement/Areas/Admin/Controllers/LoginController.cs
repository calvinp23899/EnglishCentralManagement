using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    [AllowAnonymous]
    [Route("admin")]
    public class LoginController : AdminBaseController
    {
        private readonly IAuthService _authService;
        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var account = await _authService.Login(model.Username, model.Password);

            if (account == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View("Login");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, string.Join(" ",account.FirstName,account.LastName).Trim()),
                new Claim(ClaimTypes.Role, account.Role),
                new Claim("StaffId", account.StaffId.ToString()),
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
            if (account.Role == RoleType.Teacher.GetDisplayEnumName())
            {
                return RedirectToAction("MyClass", "Class", new { area = "Admin" });
            }

            if (account.Role == RoleType.HR.GetDisplayEnumName())
            {
                return RedirectToAction("Index", "Teacher", new { area = "Admin" });
            }

            if (account.Role == RoleType.Coordinator.GetDisplayEnumName())
            {
                return RedirectToAction("Index", "Class", new { area = "Admin" });
            }
            //Auto Admim, Manager
            return RedirectToAction(
                    "Index",
                    "Home",
                    new { area = "Admin" }
            );
        }
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            HttpContext.Session.Clear();
            return RedirectToAction(
                    "Login",
                    "login",
                    new { area = "Admin" }
            );
        }

        [HttpGet("access-denied")]
        public IActionResult AccessDenied(string? returnUrl = null)
        {
            ViewBag.Error = "Bạn không có quyền truy cập vào trang này";
            return View();
        }
    }
}
