using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class TeacherController : AdminBaseController
    {
        private readonly IStaffService _staffService;
        private readonly IAccountService _accountService;
        private readonly IExcelService _excelService;

        public TeacherController(IStaffService staffService
            , IAccountService accountService,
            IExcelService excelService)
        {
            _staffService = staffService;
            _accountService = accountService;
            _excelService = excelService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {

            var result = await _staffService.GetAllAsync(page, pageSize) ?? null;

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatedStaffDto newStaff)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ToastMessage"] = "Invalid Input Form. Please Try Again";
                    TempData["ToastType"] = "error";
                    return View(newStaff);
                }

                await _staffService.CreateAsync(newStaff);
                TempData["ToastMessage"] = "Create staff successfully!";
                TempData["ToastType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return View(newStaff);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            try
            {
                var data = await _accountService.GetByStaffIdAsync(id);
                return View(data);

            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreatedStaffDto updatedStaff)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ToastMessage"] = "Invalid Input Form. Please Try Again";
                    TempData["ToastType"] = "error";
                    return View(updatedStaff);
                }
                var data = await _staffService.UpdateAsync(updatedStaff);
                //TODO: Update Account

                TempData["ToastMessage"] = "Update staff successfully!";
                TempData["ToastType"] = "success";
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return View(updatedStaff);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailId(long id)
        {
            try
            {
                var data = await _staffService.GetByIdAsync(id);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _staffService.SoftDeleteAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return BadRequest(new { message = "Confirm password not match" });
        }

        [HttpPost]
        public async Task<IActionResult> DownloadStaffExcel()
        {
            try
            {
                var data = await _staffService.GetStaffForExcel();
                var fileBytes = _excelService.ExportStaffExcel(data);

                return File(
                    fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Staff_List.xlsx"
                );
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
