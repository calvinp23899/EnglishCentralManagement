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

        public TeacherController(IStaffService staffService)
        {
            _staffService = staffService;
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
        public async Task<IActionResult> GetDetailId(long id)
        {
            var data = await _staffService.GetByIdAsync(id);
            return View(data);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            return null;
        }
    }
}
