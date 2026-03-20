using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Dtos.Setting;
using EnglishCentralManagement.Dtos.User;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface ISettingService
    {
        Task<PagedResult<SectionDto>> GetListSectionAsync(int pageIndex, int pageSize);
        Task<List<NavItemDto>> GetNavSectionAsync();
        Task<List<SliderItemDto>> GetSliderAsync();
        Task<SectionDto> GetSectionDetailAsync(long id);
        Task CreateSection(SectionDto newSection);
        Task EditSection(SectionDto updateSection);
        Task DeleteSection(long id);

        //Footer
        Task<PagedResult<FooterItemDto>> GetListFooterAsync(int pageIndex, int pageSize);
        Task<FooterItemDto> GetFooterDetailAsync(long id);
        Task<FooterDto> GetFooterDataAsync();
        Task EditFooter(FooterItemDto updateSection);
        Task CreateFooter(FooterItemDto newSection);
        Task DeleteFooter(long id);

        //SectionContentItems
        Task<PagedResult<SectionContentItemDto>> GetListContentAsync(int pageIndex, int pageSize);
        Task<SectionContentItemDto> GetContentDetailAsync(long id);
        Task<List<SectionContentDto>> GetContentDataAsync();
        Task EditContent(SectionContentItemDto updateContent);
        Task CreateContent(SectionContentItemDto newContent);
        Task DeleteContent(long id);
    }
}
