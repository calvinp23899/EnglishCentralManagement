using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Dtos.Setting;
using EnglishCentralManagement.Dtos.User;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface ISettingService
    {
        Task<PagedResult<SectionDto>> GetListSectionAsync(int pageIndex, int pageSize);
        Task<(List<string>, int)> GetDataSectionAsync(int pageIndex, int pageSize);
        Task<List<NavItemDto>> GetNavSectionAsync();
        Task<List<SliderItemDto>> GetSliderAsync();
        Task<SectionDto> GetSectionDetailAsync(long id);
        Task CreateSection(SectionDto newSection);
        Task EditSection(SectionDto updateSection);
    }
}
