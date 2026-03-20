namespace EnglishCentralManagement.Models
{
    public class SectionItem : BaseModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public string? ImageUrl { get; set; }
        public int? Order { get; set; }
        public long? HeaderBodySectionId { get; set; }
        public HeaderBodySection? HeaderBodySection { get; set; }
    }
}
