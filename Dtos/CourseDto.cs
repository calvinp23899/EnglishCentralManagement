namespace EnglishCentralManagement.Dtos
{
    public class CourseDto
    {
        public long Id { get; set; }
        public string CourseName { get; set; }
        public int DurationInMonth { get; set; }
        public decimal? MonthlyFee { get; set; }
        public decimal? Total
        {
            get
            {
                var total = DurationInMonth * MonthlyFee;
                return total;
            }
        }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
    }
}
