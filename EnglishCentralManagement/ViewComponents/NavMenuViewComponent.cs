using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.ViewComponents
{
    public class NavMenuViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;

        public NavMenuViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var navItems = await _settingService.GetNavSectionAsync();
                return View("MenuView", navItems);
            }
            catch (Exception)
            {
                var mockData = new List<NavItemDto>();
                mockData.Add(new NavItemDto
                {
                    Title = "NavItem1"
                });
                mockData.Add(new NavItemDto
                {
                    Title = "NavItem2"
                });
                mockData.Add(new NavItemDto
                {
                    Title = "NavItem3"
                });
                return View("MenuView", mockData);
            }
        }
    }
}
