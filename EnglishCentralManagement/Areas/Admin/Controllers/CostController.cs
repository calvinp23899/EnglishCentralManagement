using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos.Expense;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    [Authorize(Policy = "DashboardPolicy")]
    public class CostController : AdminBaseController
    {
        private readonly IExpenseService _expenseService;
        private readonly IPaymentService _paymentService;

        public CostController(IExpenseService expenseService, IPaymentService paymentService)
        {
            _expenseService = expenseService;
            _paymentService = paymentService;
        }
        #region Tab Overview
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> RevenueChart()
        {
            var data = await _paymentService.GetDataChart();

            return Json(data.Select(x => x.Total));
        }
        #endregion

        #region Tab Expense
        [HttpPost]
        public async Task<IActionResult> NewExpense(CreatedExpenseDto newExpense)
        {
            try
            {
                await _expenseService.AddNewExpense(newExpense);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadExpense(int page = 1, int month = 13, int year = 0)
        {
            try
            {
                if (year == 0)
                    year = DateTime.Now.Year;

                ViewBag.SelectedYear = year;
                ViewBag.SelectedMonth = month;
                var data = await _expenseService.GetAllExpense(page, 10, month, year);

                var model = new ExpenseTabDto
                {
                    Expenses = data
                };
                return PartialView("~/Areas/Admin/Views/Cost/_ExpenseViewLayout.cshtml", model);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenseById(long id)
        {
            try
            {
                var data = await _expenseService.GetExpenseById(id);

                return Json(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateExpense(ExpenseEditDto updateExpense)
        {
            try
            {
                await _expenseService.EditExpense(updateExpense);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _expenseService.DeleteExpense(id);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExpenseThisYearChart(int year)
        {
            try
            {
                var data = await _expenseService.GetExpenseChartAsync(year);
                return Json(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExpenseBreakdownChart(int month = 1, int year = 0)
        {
            try
            {
                var data = await _expenseService.GetExpenseBreakdownAsync(month, year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }

        }
        #endregion

        #region Tab Profit
        [HttpGet]
        public async Task<IActionResult> LoadProfit(int year = 0)
        {
            try
            {
                if (year == 0)
                    year = DateTime.Now.Year;

                ViewBag.SelectedProfitYear = year;
                var data = await _expenseService.GetProfitDashboardAsync(year);
                return PartialView("~/Areas/Admin/Views/Cost/_ProfitViewLayout.cshtml", data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public async Task<IActionResult> ComparedChart(int year)
        {
            try
            {
                var data = await _expenseService.GetRevenueExpenseChartAsync(year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProfitTrendChart(int year)
        {
            try
            {
                var data = await _expenseService.GetProfitTrendChartAsync(year);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        #endregion
    }
}
