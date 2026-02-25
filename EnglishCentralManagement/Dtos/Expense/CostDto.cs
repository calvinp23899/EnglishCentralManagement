using EnglishCentralManagement.Dtos.Pagination;

namespace EnglishCentralManagement.Dtos.Expense
{
    public class CostDto
    {
        public int CountClassLoss { get; set; }
        public int ThisYear
        {
            get
            {
                var year = DateTime.Today.Year;
                return year;
            }
        }
        public PagedResult<ExpenseDto> Expenses { get; set; } = new PagedResult<ExpenseDto>();
    }

}
