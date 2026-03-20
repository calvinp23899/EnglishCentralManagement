namespace EnglishCentralManagement.Dtos.User
{
    public class FooterDto
    {
        public List<ItemDefaultFooter> ItemsDefault { get; set; }
        public List<ItemDefaultFooter> ItemsContactDefault { get; set; }
        public List<ItemDefaultFooter> ItemsBranchFooter { get; set; }
        public List<ItemDefaultFooter> ItemsCourseFooter { get; set; }
        public List<ItemDefaultFooter> ItemsContactFooter { get; set; }
    }

    public class ItemDefaultFooter
    {
        public string? Description { get; set; }
        public int? Order { get; set; }
        public string? Link { get; set; }
        public string? Icon { get; set; }
        public string? IconName { get; set; }
        public string? Type { get; set; }
    }
}
