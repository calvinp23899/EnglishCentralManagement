using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Dtos.Email;
using EnglishCentralManagement.Dtos.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    [Authorize(Policy = "ManageClassPolicy")]
    public class EnrollmentController : AdminBaseController
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly IReceiptService _receiptService;
        private readonly IEmailService _emailService;

        public EnrollmentController(IEnrollmentService enrollmentService, IReceiptService receiptService, IEmailService emailService)
        {
            _enrollmentService = enrollmentService;
            _receiptService = receiptService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string search = null)
        {
            try
            {
                var data = await _enrollmentService.GetAllAsync(page, pageSize, search);
                ViewBag.enrollmentSearch = search;
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return View(new PagedResult<EnrollmentDto>());
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmailInvoice(string email)
        {
            try
            {
                var data = new GmailDto();
                //Required
                data.Subject = "Biên Lai Thu Phí";
                data.CustomerName = "Nguyen Van A";
                data.CustomerEmail = "Test@gmail.com";
                //Interview
                data.Position = "Developer";
                data.Department = "DG1";
                //Invoice
                data.InvoiceId = "TF-1-001";
                data.ProductName = "Thu Phí Lần 1";
                data.ProductDescription = "Thu Phí Lần 1";
                data.ProductAmount = 200000;
                data.PaymentMethod = "Cash";
                data.PaymentDate = DateTime.Now;
                await _emailService.SendAsync(data, Models.Enum.TemplateEmailEnum.InterviewFail);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
