using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;

        public FooterViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var data = await _settingService.GetFooterDataAsync();
                return View("FooterView", data);
            }
            catch (Exception)
            {
                var mockData = new FooterDto();
                return View("FooterView", mockData);
            }
        }
    }
}
