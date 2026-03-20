using EnglishCentralManagement.Models.Enum;

namespace EnglishCentralManagement.Dtos.Setting
{
    public class SectionContentItemDto
    {
        public long? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? Status { get; set; }
        public string? ImageUrl { get; set; }
        public int? Order { get; set; }
        public long? HeaderBodySectionId { get; set; }
        public string? NavTitleSection { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public IFormFile? Image { get; set; }
        public StatusEnum? IsActive { get; set; }
    }
}
