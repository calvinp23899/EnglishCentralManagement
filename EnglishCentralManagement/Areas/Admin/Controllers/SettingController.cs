using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos.Setting;
using EnglishCentralManagement.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public async Task<IActionResult> Index()
        {
            try
            {
                var sections = await _settingService.GetNavSectionAsync();
                ViewBag.Sections = new SelectList(sections ?? new List<NavItemDto>(), "Id", "Title");
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }

        #region Section 
        [HttpGet]
        public async Task<IActionResult> GetSections()
        {
            var sections = await _settingService.GetNavSectionAsync();
            return Json(sections ?? new List<NavItemDto>());
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
        public async Task<IActionResult> DeleteSection(long id)
        {
            try
            {
                await _settingService.DeleteSection(id);
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
        #endregion

        #region Footer
        [HttpPost]
        public async Task<IActionResult> DeleteFooter(long id)
        {
            try
            {
                await _settingService.DeleteFooter(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadFooter(int page = 1)
        {
            try
            {
                var data = await _settingService.GetListFooterAsync(page, 10);
                return PartialView("~/Areas/Admin/Views/Setting/_footerViewLayout.cshtml", data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFooter([FromForm] FooterItemDto newFooter)
        {
            try
            {
                await _settingService.CreateFooter(newFooter);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFooter([FromForm] FooterItemDto updateFooter)
        {
            try
            {
                await _settingService.EditFooter(updateFooter);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFooterDetail(long id)
        {
            try
            {
                var data = await _settingService.GetFooterDetailAsync(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Section Content

        [HttpGet]
        public async Task<IActionResult> LoadSectionContentItems(int page = 1)
        {
            try
            {
                var data = await _settingService.GetListContentAsync(page, 10);
                return PartialView("~/Areas/Admin/Views/Setting/_sectionContentViewLayout.cshtml", data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSectionItem(long id)
        {
            try
            {
                await _settingService.DeleteContent(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadSectionItem(int page = 1)
        {
            try
            {
                var data = await _settingService.GetListSectionAsync(page, 10);
                return PartialView("~/Areas/Admin/Views/Setting/_navViewLayout.cshtml", data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSectionContent([FromForm] SectionContentItemDto newSectionContent)
        {
            try
            {
                await _settingService.CreateContent(newSectionContent);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSectionContent([FromForm] SectionContentItemDto updateContent)
        {
            try
            {
                await _settingService.EditContent(updateContent);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetContentDetail(long id)
        {
            try
            {
                var data = await _settingService.GetContentDetailAsync(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
    }
}
