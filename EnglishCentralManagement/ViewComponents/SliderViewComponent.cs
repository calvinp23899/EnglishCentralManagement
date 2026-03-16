using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;

        public SliderViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var items = await _settingService.GetSliderAsync();
                return View("SliderView", items);
            }
            catch (Exception)
            {
                var mockData = new List<SliderItemDto>();
                mockData.Add(new SliderItemDto
                {
                    ImageUrl = "/assets/img/slider.png",
                    Link = "none"
                });
                return View("SliderView", mockData);
            }
        }
    }
}
