using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class EnrollmentController : AdminBaseController
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IReceiptService _receiptService;

        public EnrollmentController(IEnrollmentService enrollmentService, IReceiptService receiptService)
        {
            _enrollmentService = enrollmentService;
            _receiptService = receiptService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            try
            {
                var data = await _enrollmentService.GetAllAsync(page, pageSize);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailById(long id)
        {
            try
            {
                var data = await _enrollmentService.GetDetailById(id, 1, 10);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DownloadInvoice(long id)
        {
            try
            {
                var data = await _enrollmentService.GetDetailInvoice(id);
                var pdf = _receiptService.GenerateTuitionReceipt(data);
                string safeName = data.StudentName.Replace(" ", "-");
                string safeDate = data.Date.ToString("dd/MM/yyyy").Replace("_", "-");
                return File(
                    pdf,
                    "application/pdf",
                     $"BienLaiHocPhi-{safeName}-{safeDate}.pdf"
                );
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
                return RedirectToAction("GetDetailById");
            }

        }
    }
}
