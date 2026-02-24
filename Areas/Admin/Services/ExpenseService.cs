using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos.Expense;
using EnglishCentralManagement.Dtos.Pagination;
using EnglishCentralManagement.Dtos.Profit;
using EnglishCentralManagement.Extensions;
using EnglishCentralManagement.Helpers;
using EnglishCentralManagement.Models;
using EnglishCentralManagement.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly EnglishCentreDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public ExpenseService(EnglishCentreDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task AddNewExpense(CreatedExpenseDto newExpense)
        {
            ValidateNewExpense(newExpense);
            var model = new Expense
            {
                Amount = newExpense.Amount,
                Category = newExpense.Category,
                ExpenseDate = newExpense.ExpenseDate.ToUtcDb(),
                ClassId = newExpense.ClassId,
                Note = newExpense.Note,
                Type = newExpense.Type,
                CreatedBy = _currentUser.FullName,
                CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
            };
            _context.Expenses.Add(model);
            await _context.SaveChangesAsync();
        }

        public Task CopyExpense(long expenseId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteExpense(long id)
        {
            var model = await GetByIdAsync(id);
            model.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task EditExpense(ExpenseEditDto updateExpense)
        {
            var model = await GetByIdAsync(updateExpense.Id);

            model.Category = (ExpenseCategory)updateExpense.Category;
            model.Type = (ExpenseType)updateExpense.Type;
            model.Amount = updateExpense.Amount;
            model.Note = updateExpense.Note;
            if (!string.IsNullOrEmpty(updateExpense.ExpenseDate))
            {
                model.ExpenseDate = DateTimeOffset.Parse(updateExpense.ExpenseDate).ToUtcDb();
            }
            model.UpdatedDate = DateTimeOffset.UtcNow.ToUtcDb();
            model.UpdatedBy = _currentUser.FullName;
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ExpenseDto>> GetAllExpense(int pageIndex, int pageSize, int month = 0, int year = 0)
        {
            if (year == 0)
                year = DateTime.Now.Year;
            var filterDate = FilterMonthAndYear(month, year);
            var query = _context.Expenses
                .Include(x => x.Class)
                .Where(x => !x.IsDeleted
                && x.ExpenseDate >= filterDate.Item1 &&
                            x.ExpenseDate < filterDate.Item2);

            var totalRecords = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ExpenseDto
                {
                    Id = x.Id,
                    Category = x.Category.GetDisplayEnumName(),
                    Type = x.Type.GetDisplayEnumName(),
                    Date = x.ExpenseDate.ToVnTime(),
                    Note = x.Note,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate.Value.ToVnTime(),
                    Amount = x.Amount
                })
                .ToListAsync();
            return new PagedResult<ExpenseDto>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public async Task<ExpensePieChartDto> GetExpenseBreakdownAsync(int month, int year)
        {
            if (year == 0)
                year = DateTime.Now.Year;
            var filterDate = FilterMonthAndYear(month, year);
            var query = _context.Expenses
            .Where(x => x.ExpenseDate >= filterDate.Item1 &&
                            x.ExpenseDate < filterDate.Item2);

            var groupedData = await query
                .GroupBy(x => x.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Total = g.Sum(x => x.Amount)
                })
                .ToListAsync();
            var data = new ExpensePieChartDto
            {
                Labels = groupedData.Select(x => x.Category.GetDisplayEnumName()).ToList(),
                Data = groupedData.Select(x => x.Total).ToList()
            };
            return data;
        }

        public async Task<ExpenseEditDto> GetExpenseById(long expenseId)
        {
            var model = await GetByIdAsync(expenseId);
            var data = new ExpenseEditDto
            {
                Id = model.Id,
                Amount = model.Amount,
                Category = (int)model.Category,
                Type = (int)model.Type,
                ExpenseDate = model.ExpenseDate.ToVnTime().ToString("yyyy-MM-dd"),
                Note = model.Note
            };
            return data;
        }

        public async Task<List<decimal>> GetExpenseChartAsync(int year)
        {
            if (year == 0)
                year = DateTime.Now.Year;
            var startDate = new DateTimeOffset(
                            new DateTime(year, 1, 1),
                            TimeSpan.Zero);

            var endDate = startDate.AddYears(1);

            var monthlyExpense = await _context.Expenses
                .Where(x => x.ExpenseDate >= startDate &&
                            x.ExpenseDate < endDate)
                .GroupBy(x => x.ExpenseDate.Month)
                .Select(g => new ExpenseBarChartDto
                {
                    Month = g.Key,
                    Total = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            var result = Enumerable.Range(1, 12)
                .Select(m => monthlyExpense
                    .FirstOrDefault(x => x.Month == m)?.Total ?? 0)
                .ToList();

            return result;
        }

        private async Task<Expense> GetByIdAsync(long id)
        {
            var model = await _context.Expenses
                .Include(x => x.Class)
                .Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (model == null)
                throw new Exception("Not found Expense");
            return model;
        }
        private void ValidateNewExpense(CreatedExpenseDto data)
        {
            if (data.ExpenseDate == null)
                throw new Exception("Date Expense is required");
            if (data.Type == 0)
                throw new Exception("Type Expense is required");
            if (data.Category == 0)
                throw new Exception("Category Expense is required");
            if (data.Amount == 0)
                throw new Exception("Amount Expense is required");
        }

        private (DateTimeOffset, DateTimeOffset) FilterMonthAndYear(int month = 0, int year = 0)
        {
            var startDate = new DateTimeOffset();
            var endDate = new DateTimeOffset();
            if (month == 13)
            {
                startDate = new DateTimeOffset(
                                new DateTime(year, 1, 1),
                                TimeSpan.Zero);
                endDate = startDate.AddYears(1);
            }
            else
            {
                startDate = new DateTimeOffset(
                                new DateTime(year, month, 1),
                                TimeSpan.Zero);
                endDate = startDate.AddMonths(1);
            }
            return (startDate, endDate);
        }

        public async Task<ProfitViewDto> GetProfitDashboardAsync(int year)
        {
            if (year == 0)
                year = DateTime.Now.Year;
            var dateFilter = FilterMonthAndYear(13, year);
            var totalRevenue = await _context.Payments
                        .Where(p => p.PaidAt >= dateFilter.Item1 && p.PaidAt < dateFilter.Item2)
                        .SumAsync(p => (decimal?)p.PaidAmount) ?? 0;
            var totalExpense = await _context.Expenses
                        .Where(e => e.ExpenseDate >= dateFilter.Item1 && e.ExpenseDate < dateFilter.Item2)
                        .SumAsync(e => (decimal?)e.Amount) ?? 0;
            var totalProfit = totalRevenue - totalExpense;
            var margin = totalRevenue == 0
                ? 0
                : Math.Round((totalProfit / totalRevenue) * 100, 3);
            var data = new ProfitViewDto
            {
                TotalProfit = totalProfit,
                ProfitMargin = margin,
                TotalRevenue = totalRevenue,
            };
            return data;
        }

        public async Task<CompareChartDto> GetRevenueExpenseChartAsync(int year)
        {
            if (year == 0)
                year = DateTime.Now.Year;
            var dateFilter = FilterMonthAndYear(13, year);
            // Revenue theo tháng
            var revenueData = await _context.Payments
                .Where(p => p.PaidAt >= dateFilter.Item1 && p.PaidAt < dateFilter.Item2)
                .GroupBy(p => p.PaidAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(x => x.PaidAmount)
                })
                .ToListAsync();

            // Expense theo tháng
            var expenseData = await _context.Expenses
                .Where(e => e.ExpenseDate >= dateFilter.Item1 && e.ExpenseDate < dateFilter.Item2)
                .GroupBy(e => e.ExpenseDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            var labels = Enumerable.Range(1, 12)
                .Select(m => $"Month {m}")
                .ToList();

            var revenue = new List<decimal>();
            var expense = new List<decimal>();

            for (int month = 1; month <= 12; month++)
            {
                revenue.Add(revenueData.FirstOrDefault(x => x.Month == month)?.Total ?? 0);
                expense.Add(expenseData.FirstOrDefault(x => x.Month == month)?.Total ?? 0);
            }

            return new CompareChartDto
            {
                Labels = labels,
                RevenueData = revenue,
                ExpenseData = expense
            };
        }

        public async Task<ProfitTrendChartDto> GetProfitTrendChartAsync(int year)
        {
            if (year == 0)
                year = DateTime.Now.Year;
            var dateFilter = FilterMonthAndYear(13, year);
            // Revenue theo tháng
            var revenue = await _context.Payments
                .Where(p => p.PaidAt >= dateFilter.Item1 && p.PaidAt < dateFilter.Item2)
                .GroupBy(p => p.PaidAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(x => x.PaidAmount)
                })
                .ToListAsync();

            // Expense theo tháng
            var expense = await _context.Expenses
                .Where(e => e.ExpenseDate >= dateFilter.Item1 && e.ExpenseDate < dateFilter.Item2)
                .GroupBy(e => e.ExpenseDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            var profitList = new List<decimal>();

            for (int month = 1; month <= 12; month++)
            {
                var rev = revenue.FirstOrDefault(x => x.Month == month)?.Total ?? 0;
                var exp = expense.FirstOrDefault(x => x.Month == month)?.Total ?? 0;

                profitList.Add(rev - exp);
            }

            return new ProfitTrendChartDto
            {
                Labels = Enumerable.Range(1, 12)
                    .Select(m => $"Month {m}")
                    .ToList(),
                ProfitData = profitList
            };
        }
    }
}
