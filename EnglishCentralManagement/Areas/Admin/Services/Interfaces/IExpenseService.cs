using EnglishCentralManagement.Dtos.Expense;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Dtos.Profit;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IExpenseService
    {
        Task AddNewExpense(CreatedExpenseDto newExpense);
        Task DeleteExpense(long id);
        Task EditExpense(ExpenseEditDto updateExpense);
        Task CopyExpense(long expenseId);
        Task<ExpenseEditDto> GetExpenseById(long expenseId);
        Task<PagedResult<ExpenseDto>> GetAllExpense(int pageIndex, int pageSize, int month, int year);
        Task<List<decimal>> GetExpenseChartAsync(int year);
        Task<ExpensePieChartDto> GetExpenseBreakdownAsync(int month, int year);

        //Profit
        Task<ProfitViewDto> GetProfitDashboardAsync(int year);
        Task<CompareChartDto> GetRevenueExpenseChartAsync(int year);
        Task<ProfitTrendChartDto> GetProfitTrendChartAsync(int year);

    }
}
