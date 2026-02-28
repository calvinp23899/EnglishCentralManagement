using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class ClassController : AdminBaseController
    {
        private readonly IClassService _classService;
        private readonly IAccountService _accountService;
        private readonly ICourseService _courseService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IStudentService _studentService;
        private readonly IPaymentService _paymentService;
        private readonly IClassSessionService _classSessionService;

        public ClassController(
            IClassService classService,
            IAccountService accountService,
            ICourseService courseService,
            IEnrollmentService enrollmentService,
            IStudentService studentService,
            IPaymentService paymentService,
            IClassSessionService classSessionService
        )
        {
            _classService = classService;
            _accountService = accountService;
            _courseService = courseService;
            _enrollmentService = enrollmentService;
            _studentService = studentService;
            _paymentService = paymentService;
            _classSessionService = classSessionService;
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
                    TempData["ToastMessage"] = "Create class successfully!";
                    TempData["ToastType"] = "success";
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
                await _paymentService.AddStudentInClass(studentId, classId);
                TempData["ToastMessage"] = "Create student to class successfully!";
                TempData["ToastType"] = "success";
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return Json(new { success = false });
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
                TempData["ToastMessage"] = "Delete class successfully!";
                TempData["ToastType"] = "success";
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

        [HttpGet]
        public async Task<IActionResult> LoadSessionTab(long classId, string searchSessionName = null, int page = 1, int pageSize = 10)
        {
            try
            {
                var data = await _classSessionService.GetAllClassSession(classId, page, pageSize, searchSessionName);
                return PartialView("~/Areas/Admin/Views/Class/_ClassSessionView.cshtml", data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public async Task<IActionResult> CreateSession(CreateSessionDto createSession)
        {
            try
            {
                await _classSessionService.CreateSessionClass(createSession);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetSessionDetail(long id)
        {
            try
            {
                var data = await _classSessionService.GetSessionDetail(id);
                return Json(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSession(UpdateSessionDto updateSession)
        {
            try
            {
                await _classSessionService.UpdateSessionDetail(updateSession);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSession(long id)
        {
            try
            {
                await _classSessionService.DeleteSession(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentsBySession(long id, long classId)
        {
            try
            {
                var data = await _classSessionService.GetStudentsBySession(id, classId);
                return Json(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> SaveAttendanceSession(SaveAttendanceDto request)
        {
            if (request == null || request.Students == null)
                return BadRequest();
            try
            {
                await _classSessionService.SaveAttendance(request.SessionId, request.Students);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return BadRequest();
        }


    }
}
