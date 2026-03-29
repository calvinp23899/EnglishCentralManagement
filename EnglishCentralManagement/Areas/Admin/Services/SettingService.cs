using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Dtos.Setting;
using EnglishCentralManagement.Dtos.User;
using EnglishCentralManagement.Extensions;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models;
using EnglishCentralManagement.Models.Constants;
using EnglishCentralManagement.Models.Enum;
using EnglishCentralManagement.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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

        public async Task<List<NavItemDto>> GetNavSectionAsync()
        {
            var data = await _context.HeaderBodyItems
                            .Where(x => x.IsNav == true && !x.IsDeleted)
                            .OrderBy(x => x.Order)
                            .Select(x => new NavItemDto
                            {
                                Title = x.NavTitle,
                                Link = x.Link,
                                Id = x.Id
                            })
                            .ToListAsync();
            return data;
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
        public async Task<FooterDto> GetFooterDataAsync()
        {
            var data = new FooterDto();

            var baseQuery = _context.FooterItems
                .Where(x => !x.IsDeleted && x.IsActive == true)
                .OrderBy(x => x.Order);

            var allItems = await baseQuery
                .Select(x => new ItemDefaultFooter
                {
                    Description = x.Description,
                    Order = (int)x.Order,
                    Icon = Common.GetDescriptionEnum(Enum.Parse<IconEnum>(x.Icon)),
                    IconName = Common.GetDisplayEnumName(Enum.Parse<IconEnum>(x.Icon)),
                    Link = x.Link,
                    Type = x.IsCourse == true ? FooterTypeEnum.Course.GetDisplayEnumName()
                         : x.IsBranch == true ? FooterTypeEnum.Branch.GetDisplayEnumName()
                         : x.IsContact == true ? FooterTypeEnum.Contact.GetDisplayEnumName()
                         : "Default"
                })
                .ToListAsync();

            var contactIconDefault = new[]
            {
                IconEnum.Zalo.GetDisplayEnumName().ToLower(),
                IconEnum.Phone.GetDisplayEnumName().ToLower()
            };

            var defaultIcons = new[]
            {
                IconEnum.Gmail.GetDisplayEnumName().ToLower(),
                IconEnum.Clock.GetDisplayEnumName().ToLower()
            };

            data.ItemsDefault = allItems
                .Where(x => x.Type == "Default" && defaultIcons.Contains(x.IconName.ToLower()))
                .OrderBy(x => x.Order)
                .ToList();

            data.ItemsContactDefault = allItems
                .Where(x => x.Type == "Default" && contactIconDefault.Contains(x.IconName.ToLower()))
                .OrderBy(x => x.Order)
                .ToList();
            data.ItemsContactFooter = allItems.Where(x => x.Type.ToLower() == FooterTypeEnum.Contact.GetDisplayEnumName().ToLower())
                .OrderBy(x => x.Order)
                .ToList();
            data.ItemsCourseFooter = allItems.Where(x => x.Type.ToLower() == FooterTypeEnum.Course.GetDisplayEnumName().ToLower())
                .OrderBy(x => x.Order)
                .ToList();
            data.ItemsBranchFooter = allItems.Where(x => x.Type.ToLower() == FooterTypeEnum.Branch.GetDisplayEnumName().ToLower())
                .OrderBy(x => x.Order)
                .ToList();
            return data;
        }

        #region Section
        public async Task DeleteSection(long id)
        {
            var model = await _context.HeaderBodyItems
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();
            if (model == null)
                throw new Exception("Section Id is not found");
            if (model.CreatedBy.ToLower() == CommonConstant.BySystem.ToLower())
            {
                throw new Exception("Cannot delete section default created by system");
            }
            _context.HeaderBodyItems.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<SectionDto> GetSectionDetailAsync(long id)
        {
            var model = await _context.HeaderBodyItems
                .Where(x => x.Id == id)
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
                IsActive = model.IsDeleted == false ? StatusEnum.Active : StatusEnum.InActive,
                ImageUrl = model.ImageUrl,
            };
            return data;
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
                .Where(x => x.Id == updateSection.HeaderBodySectionId)
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
            data.IsDeleted = updateSection.IsActive == StatusEnum.Active ? false : true;
            if (data.IsSlider == true && updateSection.Image != null)
            {
                ValidateImageUpload(updateSection.Image);
                if (!string.IsNullOrEmpty(data.ImageUrl) && !data.ImageUrl.Contains("assets"))
                {
                    await _r2Service.DeleteImageAsync(data.ImageUrl);
                }
                imageUrl = await _r2Service.UploadImageAsync(updateSection.Image, FolderCloudFare.User.GetDisplayEnumName().ToLower());
                data.ImageUrl = imageUrl;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<SectionDto>> GetListSectionAsync(int pageIndex, int pageSize)
        {
            var query = _context.HeaderBodyItems
                .Where(x => x.IsDeleted == false || x.IsDeleted == true)
                .Include(x => x.SectionItems);

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
                    Type = x.IsNav == true ? "Navbar" : "Slider",
                    HasSectionItems = x.SectionItems.Any() ? true : false,
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

        #endregion

        #region Footer Service
        public async Task<PagedResult<FooterItemDto>> GetListFooterAsync(int pageIndex, int pageSize)
        {
            var query = _context.FooterItems
                .Where(x => !x.IsDeleted);

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new FooterItemDto
                {
                    Id = x.Id,
                    Description = x.Description,
                    Order = x.Order,
                    Link = x.Link,
                    Status = x.IsActive == true ? "Active" : "InActive",
                    Type = x.IsContact == true ? "Liên Hệ" : x.IsBranch == true ? "Cơ sở" : "Khoá Học",
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDate = x.UpdatedDate,
                })
                .ToListAsync();
            return new PagedResult<FooterItemDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task CreateFooter(FooterItemDto newFooter)
        {
            ValidateFooter(newFooter);
            var data = GetTypeFooter((FooterTypeEnum)newFooter.FooterEnum);
            var model = new FooterItem
            {
                Description = newFooter.Description,
                Order = newFooter.Order,
                Link = newFooter.Link,
                IsActive = true,
                Icon = newFooter.IconEnum.ToString(),
                IsBranch = data.IsBranch,
                IsContact = data.IsContact,
                IsCourse = data.IsCourse,
                CreatedBy = _currentUser.FullName,
                CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
            };
            _context.FooterItems.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFooter(long id)
        {
            var model = await _context.FooterItems
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();
            if (model == null)
                throw new Exception("Footer Id is not found");
            if (model.CreatedBy.ToLower() == CommonConstant.BySystem.ToLower())
            {
                throw new Exception("Cannot delete footer default created by system");
            }
            _context.FooterItems.Remove(model);
            await _context.SaveChangesAsync();
        }
        public async Task<FooterItemDto> GetFooterDetailAsync(long id)
        {
            var model = await _context.FooterItems
                .Where(x => !x.IsDeleted && x.Id == id)
                .FirstOrDefaultAsync();
            if (model == null)
                throw new Exception("Footer Id is not found");
            FooterTypeEnum? typeFooterModel = null;
            IconEnum? iconE = null;
            if (!string.IsNullOrEmpty(model.Icon))
                iconE = Enum.Parse<IconEnum>(model.Icon);
            if (model.IsCourse == true)
                typeFooterModel = FooterTypeEnum.Course;
            else if (model.IsBranch == true)
                typeFooterModel = FooterTypeEnum.Branch;
            else if (model.IsContact == true)
                typeFooterModel = FooterTypeEnum.Contact;
            var data = new FooterItemDto
            {
                Id = model.Id,
                Link = model.Link,
                Description = model.Description,
                Order = model.Order,
                Status = model.IsActive == true ? "Active" : "InActive",
                Icon = iconE != null ? Common.GetDescriptionEnum(iconE) : null,
                IconEnum = iconE,
                IsActive = model.IsActive == true ? StatusEnum.Active : StatusEnum.InActive,
                FooterEnum = typeFooterModel,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
                UpdatedBy = model.UpdatedBy,
                UpdatedDate = model.UpdatedDate
            };
            return data;
        }
        public async Task EditFooter(FooterItemDto updateSection)
        {
            ValidateFooter(updateSection);
            var model = await _context.FooterItems
                            .Where(x => !x.IsDeleted && x.Id == updateSection.Id)
                            .FirstOrDefaultAsync();
            if (model == null)
                throw new Exception("Footer Id is not found");
            var data = GetTypeFooter((FooterTypeEnum)updateSection.FooterEnum);
            model.Description = updateSection.Description;
            model.Link = updateSection.Link;
            model.Order = updateSection.Order;
            model.Icon = updateSection.IconEnum.ToString();
            model.IsBranch = data.IsBranch;
            model.IsActive = updateSection.IsActive == StatusEnum.Active ? true : false;
            model.IsCourse = data.IsCourse;
            model.IsContact = data.IsContact;
            model.UpdatedDate = DateTimeOffset.UtcNow.ToUtcDb();
            model.UpdatedBy = _currentUser.FullName;
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Content Items
        public async Task<PagedResult<SectionContentItemDto>> GetListContentAsync(int pageIndex, int pageSize)
        {
            var query = _context.SectionItems
                .Include(x => x.HeaderBodySection);

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new SectionContentItemDto
                {
                    Id = x.Id,
                    NavTitleSection = x.HeaderBodySection.NavTitle,
                    Title = x.Title,
                    Description = x.Description,
                    Order = x.Order,
                    Link = x.Link,
                    Status = x.IsDeleted == false ? "Active" : "InActive",
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdatedDate = x.UpdatedDate,
                })
                .ToListAsync();
            return new PagedResult<SectionContentItemDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<SectionContentItemDto> GetContentDetailAsync(long id)
        {
            var data = await _context.SectionItems
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new Exception("Content not found");
            var model = new SectionContentItemDto
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                Order = data.Order,
                Link = data.Link,
                HeaderBodySectionId = data.HeaderBodySectionId,
                ImageUrl = data.ImageUrl,
                Status = data.IsDeleted == true ? "InActive" : "Active",
                IsActive = data.IsDeleted == true ? StatusEnum.InActive : StatusEnum.Active,
            };
            return model;
        }

        public async Task<List<SectionContentDto>> GetContentDataAsync()
        {
            var data = await _context.HeaderBodyItems
                .Where(x => x.IsNav == true && !x.IsDeleted)
                .Include(x => x.SectionItems.Where(s => !s.IsDeleted))
                .OrderBy(x => x.Order)
                .ToListAsync();

            var grouped = data
                .GroupBy(x => x.Link ?? string.Empty)
                .Select(g => new SectionContentDto
                {
                    SectionLink = g.Key,
                    ItemsContent = g.SelectMany(x => x.SectionItems)
                        .Select(s => new ContentItemsDto
                        {
                            ContentTitle = s.Title,
                            ContentDescription = s.Description,
                            ContentLink = s.Link,
                            ContentImage = s.ImageUrl,
                        })
                        .ToList()
                })
                .ToList();

            return grouped;
        }

        public async Task EditContent(SectionContentItemDto updateContent)
        {
            var data = await _context.SectionItems
                .FirstOrDefaultAsync(x => x.Id == updateContent.Id)
                ?? throw new Exception("Content not found");

            if (updateContent.Image != null)
            {
                ValidateImageUpload(updateContent.Image);
                data.ImageUrl = await _r2Service.UploadImageAsync(updateContent.Image, FolderCloudFare.User.GetDisplayEnumName().ToLower());

                if (!string.IsNullOrWhiteSpace(data.ImageUrl))
                {
                    await _r2Service.DeleteImageAsync(data.ImageUrl);
                }
            }
            data.Title = updateContent.Title;
            data.Description = updateContent.Description;
            data.Order = updateContent.Order;
            data.Link = updateContent.Link;
            data.HeaderBodySectionId = updateContent.HeaderBodySectionId;
            data.UpdatedBy = _currentUser.FullName;
            data.UpdatedDate = DateTimeOffset.UtcNow.ToUtcDb();
            data.IsDeleted = updateContent.IsActive == StatusEnum.InActive ? true : false;

            await _context.SaveChangesAsync();
        }

        public async Task CreateContent(SectionContentItemDto newContent)
        {
            string imageUrl = string.Empty;
            if (newContent.Image != null)
            {
                ValidateImageUpload(newContent.Image);
                imageUrl = await _r2Service.UploadImageAsync(newContent.Image, FolderCloudFare.User.GetDisplayEnumName().ToLower());
            }
            var data = new SectionItem
            {
                Title = newContent.Title,
                Description = newContent.Description,
                Order = newContent.Order,
                Link = newContent.Link,
                HeaderBodySectionId = newContent.HeaderBodySectionId,
                ImageUrl = imageUrl,
                CreatedBy = _currentUser.FullName,
                CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
            };
            _context.SectionItems.Add(data);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContent(long id)
        {
            var model = await _context.SectionItems
                            .Where(x => x.Id == id)
                            .FirstOrDefaultAsync();
            if (model == null)
                throw new Exception("SectionContent Id is not found");
            if (model.CreatedBy.ToLower() == CommonConstant.BySystem.ToLower())
            {
                throw new Exception("Cannot delete footer default created by system");
            }
            _context.SectionItems.Remove(model);
            await _context.SaveChangesAsync();
        }
        #endregion


        #region Private methods

        private void ValidateSection(SectionDto data)
        {
            var fields = new List<string>();
            var regex = new Regex(@"^[a-z]+(-[a-z]+)*$");

            if (data.IsNav == true)
            {
                if (string.IsNullOrWhiteSpace(data.NavTitle))
                    fields.Add("Nav Title");
                if (string.IsNullOrWhiteSpace(data.Title))
                    fields.Add("Title");
                if (string.IsNullOrWhiteSpace(data.Description))
                    fields.Add("Description");
                if (!regex.IsMatch(data.Link))
                {
                    throw new Exception("Nếu là IsNav thì link lưu dạng section với format không dấu: abc hoặc abc-def");
                }
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

        private (bool? IsBranch, bool? IsCourse, bool? IsContact) GetTypeFooter(FooterTypeEnum type)
        {
            bool? isTypeBranch = null;
            bool? isTypeCourse = null;
            bool? isTypeContact = null;

            if (type == FooterTypeEnum.Course) isTypeCourse = true;
            if (type == FooterTypeEnum.Branch) isTypeBranch = true;
            if (type == FooterTypeEnum.Contact) isTypeContact = true;

            return (isTypeBranch, isTypeCourse, isTypeContact);
        }
        private void ValidateFooter(FooterItemDto data)
        {
            var fields = new List<string>();
            var otherMsg = new List<string>();
            if (data.FooterEnum == null)
            {
                fields.Add("FooterType");
            }
            if (data.IconEnum == null)
                otherMsg.Add("Icon is required for type branch or contact.");

            if (string.IsNullOrWhiteSpace(data.Description))
                fields.Add("Description");

            if (data.Order == null)
                fields.Add("Order");
            if (string.IsNullOrWhiteSpace(data.Link))
                otherMsg.Add("Link can be #.");
            if (fields.Count == 0 && otherMsg.Count > 0)
            {
                throw new Exception($"{string.Join(" ", otherMsg)}");
            }
            if (fields.Count > 0 || otherMsg.Count > 0)
            {
                string fieldList = string.Join(", ", fields);
                string verb = fields.Count == 1 ? "is" : "are";

                throw new Exception($"{fieldList} {verb} required. {otherMsg}");
            }
        }


        #endregion
    }
}
