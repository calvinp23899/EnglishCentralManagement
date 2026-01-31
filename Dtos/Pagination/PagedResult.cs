namespace EnglishCentralManagement.Dtos.Pagination
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int PageIndex { get; set; }     // page hiện tại (1-based)
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalRecords / PageSize);

        public bool HasPrevious => PageIndex > 1;
        public bool HasNext => PageIndex < TotalPages;
    }
}
