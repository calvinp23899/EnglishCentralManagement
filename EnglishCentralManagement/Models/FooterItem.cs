namespace EnglishCentralManagement.Models
{
    public class FooterItem : BaseModel
    {
        public string? Description { get; set; }
        public int? Order { get; set; }
        public string? Link { get; set; }
        public bool? IsActive { get; set; }
        public string? Icon { get; set; }
        public bool? IsCourse { get; set; }
        public bool? IsContact { get; set; }
        public bool? IsBranch { get; set; }
    }
}
