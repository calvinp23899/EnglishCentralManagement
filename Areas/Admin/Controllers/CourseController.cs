using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class CourseController : AdminBaseController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            try
            {
                var data = await _courseService.GetAllCourseAsync(page, pageSize);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailById(long id)
        {
            try
            {
                var data = await _courseService.ViewCourseDetailAsync(id);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var data = await _courseService.ViewCourseDetailAsync(id);
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
        public async Task<IActionResult> Edit(CourseDto updateCourse)
        {
            try
            {
                await _courseService.UpdateCourseAsync(updateCourse);
                TempData["ToastMessage"] = "Update course successfully!";
                TempData["ToastType"] = "success";
                return View(updateCourse);
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
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _courseService.DeleteCourseAsync(id);
                TempData["ToastMessage"] = "Delete course successfully!";
                TempData["ToastType"] = "success";
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseDto newCourse)
        {
            try
            {
                await _courseService.CreateCourseAsync(newCourse);
                TempData["ToastMessage"] = "Create course successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return View(newCourse);
        }
    }
}
