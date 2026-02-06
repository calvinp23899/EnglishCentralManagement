using EnglishCentralManagement.Areas.Admin.Services;
using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Models;
using EnglishCentralManagement.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class ClassController : AdminBaseController
    {
        private readonly IClassService _classService;
        private readonly IAccountService _accountService;
        private readonly ICourseService _courseService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IStudentService _studentService;

        public ClassController(
            IClassService classService,
            IAccountService accountService,
            ICourseService courseService,
            IEnrollmentService enrollmentService,
            IStudentService studentService
        )
        {
            _classService = classService;
            _accountService = accountService;
            _courseService = courseService;
            _enrollmentService = enrollmentService;
            _studentService = studentService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            try
            {
                var data = await _classService.GetAllAsync(page, pageSize);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return View();
        }

        public async Task<IActionResult> MyClass(int page = 1, int pageSize = 10)
        {
            try
            {
                var userId = CurrentUserId();
                var data = await _classService.GetAllMyClassAsync(page, pageSize, userId);
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
        public async Task<IActionResult> Create()
        {
            var data = new CreatedClassDto();
            try
            {
                var listTeacherSelected = await _accountService.GetListTeacherSelectedAsync();
                var listCourseSelected = await _courseService.GetCourseSelectedAsync();
                data.Teachers = listTeacherSelected;
                data.Courses = listCourseSelected;
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return View(data);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatedClassDto newClass)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ToastMessage"] = "Invalid Input Form. Please Try Again";
                    TempData["ToastType"] = "error";
                    return RedirectToAction("Create");
                }
                else
                {
                    await _classService.CreateAsync(newClass);

                }
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return RedirectToAction("Create");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailId(long id)
        {
            try
            {
                var listStdInClass = await _enrollmentService.GetStudentInClassAsync(1, 10, id);
                var listStdNotInClass = await _studentService.GetStudentNotInClassAsync(1, 10, id);
                var data = await _classService.GetDetailById(id);
                data.StudentsInClass = listStdInClass;
                data.StudentsAddToClass = listStdNotInClass;
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddStudentToClass(long studentId, long classId)
        {
            try
            {
                await _enrollmentService.AddStudentInClassAsync(studentId, classId);
                return Json(new { success = false });

            }
            catch (Exception ex) {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var listTeacherSelected = await _accountService.GetListTeacherSelectedAsync();
                var listCourseSelected = await _courseService.GetCourseSelectedAsync();
                var data = await _classService.GetClassInfoForEdit(id, listTeacherSelected, listCourseSelected);              
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreatedClassDto updateClass)
        {
            try
            {
                await _classService.UpdateAsync(updateClass);

                TempData["ToastMessage"] = "Update Class successfully!";
                TempData["ToastType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _classService.SoftDeleteAsync(id);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return RedirectToAction("Index");
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> MyClassDetail(long id)
        {
            try
            {
                var listStdInClass = await _enrollmentService.GetStudentInClassAsync(1, 10, id);
                var data = await _classService.GetDetailById(id);
                data.StudentsInClass = listStdInClass;
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return RedirectToAction("Index");
        }
    }
}
