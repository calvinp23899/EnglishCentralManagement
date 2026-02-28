using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
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
                return View("Index");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, string.Join(" ",account.FirstName,account.LastName).Trim()),
                new Claim(ClaimTypes.Role, account.Role),
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

        [HttpGet]
        public IActionResult AccessDenied(string? returnUrl = null)
        {
            ViewBag.Error = "Bạn không có quyền truy cập vào trang này";
            return View();
        }
    }
}
