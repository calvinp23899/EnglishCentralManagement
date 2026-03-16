namespace EnglishCentralManagement.Models
{
    public class HeaderBodySection : BaseModel
    {
        public string? Title { get; set; }
        public string? NavTitle { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? ImageUrl { get; set; }
        public int? Order { get; set; }
        public bool? IsNav { get; set; }
        public bool? IsSlider { get; set; }
        public ICollection<SectionItem> SectionItems { get; set; } = new List<SectionItem>();
    }
}
