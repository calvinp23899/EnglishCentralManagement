using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    [Authorize(Policy = "ManageClassPolicy")]
    public class PaymentController : AdminBaseController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPayment(long id, PaymentEditDto updatePayment)
        {
            try
            {
                await _paymentService.EditPayment(id, updatePayment);
                TempData["ToastMessage"] = "Update Payment Successfully!";
                TempData["ToastType"] = "success";
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return Json(new { success = false });
        }
    }
}
