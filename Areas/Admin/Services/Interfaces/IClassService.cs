using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Dtos;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IClassService
    {
        Task<PagedResult<ClassDto>> GetAllAsync(int pageIndex, int pageSize);
        Task<PagedResult<ClassDto>> GetAllMyClassAsync(int pageIndex, int pageSize);
        Task CreateAsync(CreatedClassDto newClass);
        Task<CreatedClassDto> UpdateAsync(CreatedClassDto updateClass);
        Task SoftDeleteAsync(long id);

    }
}
