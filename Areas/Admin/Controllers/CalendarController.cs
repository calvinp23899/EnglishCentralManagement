using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EnglishCentralManagement.Areas.Admin.Controllers
{
    public class CalendarController : AdminBaseController
    {
        private readonly IEventService _eventService;

        public CalendarController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? date)
        {
            try
            {
                var userId = CurrentUserId();
                var selectedDate = date ?? DateTime.Today;
                var events = await _eventService.GetAllEventById(userId);

                var model = new CalendarViewModel
                {
                    SelectedDate = selectedDate,
                    Events = events
                };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = ex.Message;
                TempData["ToastType"] = "error";
            }
            return View(new CalendarViewModel
            {
                SelectedDate = DateTime.Today,
                Events = new List<CreatedEventDto>()
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreatedEventDto newEvent)
        {
            try
            {
                await _eventService.CreateEvent(newEvent);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditEvent(CreatedEventDto updateEvent)
        {
            try
            {
                await _eventService.UpdateEvent(updateEvent);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(long EventId)
        {
            try
            {
                await _eventService.DeleteEvent(EventId);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
