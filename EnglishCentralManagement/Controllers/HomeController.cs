using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos.User;
using EnglishCentralManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Diagnostics;

namespace EnglishCentralManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStudentService _studentService;
        private readonly ISettingService _settingService;

        public HomeController(ILogger<HomeController> logger
            , IStudentService studentService
            , ISettingService settingService)
        {
            _logger = logger;
            _studentService = studentService;
            _settingService = settingService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await _settingService.GetContentDataAsync();
                return View(data);
            }
            catch (Exception)
            {
                return View(new List<SectionContentDto>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("RegisterPolicy")]
        public async Task<IActionResult> RegisterStudent(UserRegisterDto newStudent)
        {
            try
            {
                await _studentService.RegisterStudentAsync(newStudent);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
