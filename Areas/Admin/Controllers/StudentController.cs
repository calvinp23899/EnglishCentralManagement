using EnglishCentralManagement.Areas.Admin.Services;
using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class StudentController : AdminBaseController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task <IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var data = await _studentService.GetAllAsync(page, pageSize);
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatedStudentDto newStudent)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ToastMessage"] = "Invalid Input Form. Please Try Again";
                    TempData["ToastType"] = "error";
                    return View(newStudent);
                }
                await _studentService.CreateAsync(newStudent);
                TempData["ToastMessage"] = "Create teacher successfully!";
                TempData["ToastType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return View(newStudent);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var data = await _studentService.GetByIdAsync(id);
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
        public async Task<IActionResult> Edit(CreatedStudentDto updateStudent)
        {
            try
            {
                var data = await _studentService.UpdateAsync(updateStudent);
                TempData["ToastMessage"] = "Update student successfully!";
                TempData["ToastType"] = "success";
                //TODO: Update Account
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return View(updateStudent);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailId(long id)
        {
            try
            {
                //TODO: Get List StudentClass
                var data = await _studentService.GetByIdAsync(id);
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
            try
            {
                await _studentService.SoftDeleteAsync(id);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return RedirectToAction("Index");
            }
            return Json(new { success = true });
        }
    }
}
