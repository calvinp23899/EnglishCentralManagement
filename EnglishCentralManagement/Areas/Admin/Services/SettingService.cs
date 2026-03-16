using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Dtos.Setting;
using EnglishCentralManagement.Dtos.User;
using EnglishCentralManagement.Extensions;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models;
using EnglishCentralManagement.Models.Enum;
using EnglishCentralManagement.Services;
using Microsoft.EntityFrameworkCore;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class SettingService : ISettingService
    {
        private readonly EnglishCentreDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly CloudflareR2Service _r2Service;

        public SettingService(EnglishCentreDbContext context
            , ICurrentUserService currentUser
            , CloudflareR2Service r2Service
            )
        {
            _context = context;
            _currentUser = currentUser;
            _r2Service = r2Service;
        }

        public async Task CreateSection(SectionDto newSection)
        {
            ValidateSection(newSection);
            string imageUrl = null;
            if (newSection.IsNav == true)
            {
                var navCount = await _context.HeaderBodyItems
                    .CountAsync(x => x.IsNav == true && x.IsDeleted == false);
                if (navCount >= 4)
                    throw new Exception("Nav items have reached the maximum limit of 4.");
            }
            else
            {
                var sliderCount = await _context.HeaderBodyItems
                    .CountAsync(x => x.IsSlider == true && x.IsDeleted == false);
                if (sliderCount >= 3)
                    throw new Exception("Slider items have reached the maximum limit of 3.");
            }
            if (newSection.IsSlider == true)
            {
                ValidateImageUpload(newSection.Image);
                imageUrl = await _r2Service.UploadImageAsync(newSection.Image, FolderCloudFare.User.GetDisplayEnumName().ToLower());
            }
            var data = new HeaderBodySection
            {
                NavTitle = newSection.NavTitle,
                Title = newSection.Title,
                Description = newSection.Description,
                Link = newSection.Link,
                Order = newSection.Order,
                IsNav = newSection.IsNav,
                IsSlider = newSection.IsSlider,
                IsDeleted = false,
                CreatedBy = _currentUser.FullName,
                ImageUrl = imageUrl,
                CreatedDate = DateTimeOffset.UtcNow.ToUtcDb()
            };
            _context.HeaderBodyItems.Add(data);
            await _context.SaveChangesAsync();

        }

        public async Task EditSection(SectionDto updateSection)
        {
            ValidateSection(updateSection);
            string imageUrl = null;
            var data = await _context.HeaderBodyItems
                .Where(x => !x.IsDeleted && x.Id == updateSection.HeaderBodySectionId)
                .FirstOrDefaultAsync();
            if (data == null)
                throw new Exception("Section Id is not found");

            data.UpdatedDate = DateTimeOffset.UtcNow.ToUtcDb();
            data.UpdatedBy = _currentUser.FullName;
            data.NavTitle = updateSection.NavTitle;
            data.Title = updateSection.Title;
            data.Description = updateSection.Description;
            data.Link = updateSection.Link;
            data.Order = updateSection.Order;
            if (data.IsSlider == true && updateSection.Image != null)
            {
                ValidateImageUpload(updateSection.Image);
                if (!string.IsNullOrEmpty(data.ImageUrl))
                {
                    await _r2Service.DeleteImageAsync(data.ImageUrl);
                }
                imageUrl = await _r2Service.UploadImageAsync(updateSection.Image, FolderCloudFare.User.GetDisplayEnumName().ToLower());
                data.ImageUrl = imageUrl;
            }
            await _context.SaveChangesAsync();
        }

        public Task<(List<string>, int)> GetDataSectionAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<SectionDto>> GetListSectionAsync(int pageIndex, int pageSize)
        {
            var query = _context.HeaderBodyItems
                .Where(x => !x.IsDeleted);

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new SectionDto
                {
                    HeaderBodySectionId = x.Id,
                    Title = x.Title ?? "ImageSlider",
                    NavTitle = x.NavTitle ?? "ImageSlider",
                    Description = x.Description,
                    Order = x.Order,
                    Link = x.Link,
                    Status = x.IsDeleted == false ? "Active" : "InActive",
                    IsNav = x.IsNav,
                    IsSlider = x.IsSlider,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDate = x.UpdatedDate,
                    Type = x.IsNav == true ? "Navbar" : "Slider"
                })
                .ToListAsync();
            return new PagedResult<SectionDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<List<NavItemDto>> GetNavSectionAsync()
        {
            var data = await _context.HeaderBodyItems
                            .Where(x => x.IsNav == true && !x.IsDeleted)
                            .OrderBy(x => x.Order)
                            .Select(x => new NavItemDto
                            {
                                Title = x.NavTitle,
                                Link = x.Link,
                            })
                            .ToListAsync();
            return data;
        }

        public async Task<SectionDto> GetSectionDetailAsync(long id)
        {
            var model = await _context.HeaderBodyItems
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (model == null)
                throw new Exception("Section Id is not found");
            var data = new SectionDto
            {
                HeaderBodySectionId = model.Id,
                Title = model.Title,
                NavTitle = model.NavTitle,
                Link = model.Link,
                Description = model.Description,
                IsNav = model.IsNav,
                IsSlider = model.IsSlider,
                Order = model.Order,
                Status = model.IsDeleted == false ? "Active" : "InActive",
                ImageUrl = model.ImageUrl,
            };
            return data;
        }

        private void ValidateSection(SectionDto data)
        {
            var fields = new List<string>();

            if (data.IsNav == true)
            {
                if (string.IsNullOrWhiteSpace(data.NavTitle))
                    fields.Add("Nav Title");
                if (string.IsNullOrWhiteSpace(data.Title))
                    fields.Add("Title");
                if (string.IsNullOrWhiteSpace(data.Description))
                    fields.Add("Description");
            }

            if (string.IsNullOrWhiteSpace(data.Link))
                fields.Add("Link");
            if (data.Order == null)
                fields.Add("Order");

            if (fields.Count > 0)
            {
                string fieldList = string.Join(", ", fields);
                string verb = fields.Count == 1 ? "is" : "are";
                throw new Exception($"{fieldList} {verb} required.");
            }
        }

        private void ValidateImageUpload(IFormFile file)
        {
            // Giới hạn 5MB
            if (file.Length > 5 * 1024 * 1024)
                throw new Exception("Ảnh không được vượt quá 5MB!");

            // Chỉ cho phép đuôi ảnh
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(ext))
                throw new Exception("Chỉ chấp nhận file jpg, png");
        }

        public async Task<List<SliderItemDto>> GetSliderAsync()
        {
            var data = await _context.HeaderBodyItems
                            .Where(x => x.IsSlider == true && !x.IsDeleted)
                            .OrderBy(x => x.Order)
                            .Select(x => new SliderItemDto
                            {
                                ImageUrl = x.ImageUrl ?? "/assets/img/slider.png",
                                Link = x.Link,
                            })
                            .ToListAsync();
            return data;
        }
    }
}
