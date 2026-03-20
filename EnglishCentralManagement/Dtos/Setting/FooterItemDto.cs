using EnglishCentralManagement.Models.Enum;

namespace EnglishCentralManagement.Dtos.Setting
{
    public class FooterItemDto
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Type { get; set; }
        public int? Order { get; set; }
        public string? Status { get; set; }
        public string? Link { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public IconEnum? IconEnum { get; set; }
        public FooterTypeEnum? FooterEnum { get; set; }
        public StatusEnum? IsActive { get; set; }
    }
}
