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

        public ClassController(IClassService classService, IAccountService accountService, ICourseService courseService)
        {
            _classService = classService;
            _accountService = accountService;
            _courseService = courseService;
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

        public IActionResult MyClass()
        {
            var userId = CurrentUserId();
            //TODO: Get Class By UserId Loggin
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
        public async Task<IActionResult> GetDetailId()
        {
            var listStd = new List<StudentListDto>
            {
                new StudentListDto
                {
                    Id = 1L,
                    FullName = "Nguyễn Văn A",
                    Email = "a.nguyen@gmail.com",
                    PhoneNumber = "0901234567",
                    Status = "Active"
                },
                new StudentListDto
                {
                    Id = 2L,
                    FullName = "Trần Thị B",
                    Email = "b.tran@gmail.com",
                    PhoneNumber = "0912345678",
                    Status = "Inactive"
                },
                new StudentListDto
                {
                    Id = 3L,
                    FullName = "Lê Văn C",
                    Email = "c.le@gmail.com",
                    PhoneNumber = "0987654321",
                    Status = "Active"
                }
            };
            var data = new ClassDetailDto
            {
                ClassCode = "aaa",
                CourseName = "bbbb",
                MaxStudents = 3,
                EndDate = DateTime.Now,
                StartDate = DateTime.Now,
                TeacherName = "aasda",
                Status = ClassStatusEnum.Active,
                Students = new PagedResult<StudentListDto>
                {
                    Items = listStd,
                    PageIndex = 1,
                    PageSize = 10,
                    TotalRecords = 10
                }
            };
            return View(data);
        }
    }
}
