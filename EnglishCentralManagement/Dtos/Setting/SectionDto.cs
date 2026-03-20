using EnglishCentralManagement.Models.Enum;

namespace EnglishCentralManagement.Dtos.Setting
{
    public class SectionDto
    {
        public long? HeaderBodySectionId { get; set; }
        public string? Title { get; set; }
        public string? NavTitle { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Link { get; set; }
        public int? Order { get; set; }
        public bool? IsNav { get; set; }
        public bool? IsSlider { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public IFormFile? Image { get; set; }
        public string? Type { get; set; }
        public string? ImageUrl { get; set; }
        public StatusEnum? IsActive { get; set; }
        public bool? HasSectionItems { get; set; }
    }
}
