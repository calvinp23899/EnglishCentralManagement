using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        private readonly IStaffService _staffService;
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;
        private readonly IPaymentService _paymentService;

        public HomeController(
            IStaffService staffService,
            IStudentService studentService,
            IClassService classService,
            IPaymentService paymentService
        )
        {
            _staffService = staffService;
            _studentService = studentService;
            _classService = classService;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                int countStudent = await _studentService.CountAllStudent();
                int countStaff = await _staffService.CountAllStaff();
                int countClass = await _classService.CountClass();
                decimal RevenueThisMonth = await _paymentService.CalRevenueThisMonth();
                var data = new DashboardDto
                {
                    CountClass = countClass,
                    CountStudent = countStudent,
                    CountStaff = countStaff,
                    RevenueThisMonth = RevenueThisMonth,
                };
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return View();
        }


    }
}
