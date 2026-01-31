using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class TeacherController : AdminBaseController
    {
        private readonly IStaffService _staffService;
        private readonly IAccountService _accountService;

        public TeacherController(IStaffService staffService, IAccountService accountService)
        {
            _staffService = staffService;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {

            var result = await _staffService.GetAllAsync(page, pageSize) ?? null;

            return View(result);            
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatedStaffDto newStaff)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ToastMessage"] = "Invalid Input Form. Please Try Again";
                    TempData["ToastType"] = "error";
                    return View(newStaff);
                }
                
                await _staffService.CreateAsync(newStaff);
                TempData["ToastMessage"] = "Create teacher successfully!";
                TempData["ToastType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return View(newStaff);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var data = await _accountService.GetByStaffIdAsync(id);
                return View(data);

            }catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreatedStaffDto updatedStaff)
        {
            try
            {
                var data = await _staffService.UpdateAsync(updatedStaff);
            }catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return View(updatedStaff);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailId(long id)
        {
            try
            {
                var data = await _staffService.GetByIdAsync(id);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            await _staffService.SoftDeleteAsync(id);
            return Json(new { success = true });
        }
    }
}
