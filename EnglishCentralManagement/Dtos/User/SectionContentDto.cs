namespace EnglishCentralManagement.Dtos.User
{
    public class SectionContentDto
    {
        public string? SectionLink { get; set; }
        public List<ContentItemsDto> ItemsContent = new List<ContentItemsDto>();
    }

    public class ContentItemsDto
    {
        public string? ContentTitle { get; set; }
        public string? ContentDescription { get; set; }
        public string? ContentLink { get; set; }
        public string? ContentImage { get; set; }
    }
}
