using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{

    [Authorize(Policy = "ProfilePolicy")]
    public class ProfileController : AdminBaseController
    {
        private readonly IAccountService _accountService;

        public ProfileController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var id = CurrentUserId();
                var data = await _accountService.GetProfileAsync(id);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string newPassword)
        {
            try
            {
                var id = CurrentUserId();
                await _accountService.ChangePassword(id, newPassword);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return BadRequest(new { message = "Confirm password not match" });
        }
    }
}
