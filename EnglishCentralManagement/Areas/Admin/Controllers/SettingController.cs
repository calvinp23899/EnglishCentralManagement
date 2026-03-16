using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    [Authorize(Policy = "DashboardPolicy")]
    public class SettingController : AdminBaseController
    {
        private ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadSectionItem()
        {
            try
            {
                var data = await _settingService.GetListSectionAsync(1, 10);
                return PartialView("~/Areas/Admin/Views/Setting/_navViewLayout.cshtml", data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSection([FromForm] SectionDto newSection)
        {
            try
            {
                await _settingService.CreateSection(newSection);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSection([FromForm] SectionDto newSection)
        {
            try
            {
                await _settingService.EditSection(newSection);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSectionDetail(long id)
        {
            try
            {
                var data = await _settingService.GetSectionDetailAsync(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadSlider(int page = 1)
        {
            try
            {
                return PartialView("~/Areas/Admin/Views/Setting/_sliderViewLayout.cshtml");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }




        [HttpGet]
        public async Task<IActionResult> LoadCourse(int page = 1)
        {
            try
            {
                return PartialView("~/Areas/Admin/Views/Setting/_courseViewLayout.cshtml");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
