using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index(string search = null, int page = 1, int pageSize = 10)
        {
            try
            {
                var data = await _studentService.GetAllAsync(page, pageSize, search);
                ViewBag.studentSearch = search;
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return View();
            }
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
                    return View(newStudent);
                }
                await _studentService.CreateAsync(newStudent);
                TempData["ToastMessage"] = "Create teacher successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return View(newStudent);
            }
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
        public async Task<IActionResult> Edit(UpdateStudentDto updateStudent)
        {
            var dataReturn = new CreatedStudentDto();
            try
            {
                if (!ModelState.IsValid)
                {
                    dataReturn = ReturnDataCreated(updateStudent);
                    return View(dataReturn);
                }
                var data = await _studentService.UpdateAsync(updateStudent);
                TempData["ToastMessage"] = "Update student successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                dataReturn = ReturnDataCreated(updateStudent);
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return View(dataReturn);
            }
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

        private CreatedStudentDto ReturnDataCreated(UpdateStudentDto updateStudent)
        {
            var dataReturn = new CreatedStudentDto
            {
                FirstName = updateStudent.FirstName,
                LastName = updateStudent.LastName,
                Email = updateStudent.Email,
                Address = updateStudent.Address,
                DateOfBirth = updateStudent.DateOfBirth,
                Gender = updateStudent.Gender,
                PhoneNumber = updateStudent.PhoneNumber,
                Status = updateStudent.Status,
                StudentId = updateStudent.StudentId
            };
            return dataReturn;

        }
    }
}
