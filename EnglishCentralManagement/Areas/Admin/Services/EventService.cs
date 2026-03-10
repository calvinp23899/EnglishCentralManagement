using EnglishCentralManagement.Areas.Admin.Services.Interfaces;
using EnglishCentralManagement.Data;
using EnglishCentralManagement.Dtos;
using EnglishCentralManagement.Extensions;
using EnglishCentralManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishCentralManagement.Areas.Admin.Services
{
    public class EventService : IEventService
    {
        private readonly EnglishCentreDbContext _context;
        private readonly ICurrentUserService _currentUser;
        public EventService(EnglishCentreDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task CreateEvent(CreatedEventDto newEvent)
        {
            CheckValidateCreateEvent(newEvent);
            await CheckOverlapEvent(newEvent);
            var data = new EventCalendar
            {
                EventName = newEvent.EventName,
                CreatedDate = DateTimeOffset.UtcNow.ToUtcDb(),
                CreatedBy = _currentUser.FullName,
                StaffId = (long)_currentUser.UserId,
                LinkMeeting = newEvent.LinkMeeting,
                StartDate = newEvent.StartDate,
                EndDate = newEvent.EndDate,
                StartTime = newEvent.StartTime,
                EndTime = newEvent.EndTime,
                IsMonday = newEvent.IsMonday,
                IsTuesday = newEvent.IsTuesday,
                IsWednesday = newEvent.IsWednesday,
                IsThursday = newEvent.IsThursday,
                IsFriday = newEvent.IsFriday,
                IsSaturday = newEvent.IsSaturday,
                IsSunday = newEvent.IsSunday,
            };
            _context.EventCalendars.Add(data);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEvent(long id)
        {
            var model = await _context.EventCalendars
                .Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            model.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<CreatedEventDto>> GetAllEventById(long id)
        {
            var data = await _context.EventCalendars
                .Where(x => x.StaffId == id && !x.IsDeleted)
                .Select(x => new CreatedEventDto
                {
                    EventName = x.EventName,
                    LinkMeeting = x.LinkMeeting,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    IsMonday = x.IsMonday,
                    IsTuesday = x.IsTuesday,
                    IsWednesday = x.IsWednesday,
                    IsThursday = x.IsThursday,
                    IsFriday = x.IsFriday,
                    IsSaturday = x.IsSaturday,
                    IsSunday = x.IsSunday,
                    StaffId = x.StaffId,
                    EventId = x.Id
                })
                .ToListAsync();
            return data;
        }

        public async Task UpdateEvent(CreatedEventDto updateEvent)
        {
            CheckValidateCreateEvent(updateEvent);
            await CheckOverlapEvent(updateEvent);
            var model = await _context.EventCalendars
                .Where(x => x.Id == updateEvent.EventId && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if (model == null)
            {
                throw new Exception("Không tìm thấy event");
            }
            model.UpdatedDate = DateTimeOffset.UtcNow.ToUtcDb();
            model.UpdatedBy = _currentUser.FullName;
            model.EventName = updateEvent.EventName;
            model.LinkMeeting = updateEvent.LinkMeeting;
            model.StartDate = updateEvent.StartDate;
            model.EndDate = updateEvent.EndDate;
            model.StartTime = updateEvent.StartTime;
            model.EndTime = updateEvent.EndTime;
            model.IsMonday = updateEvent.IsMonday;
            model.IsTuesday = updateEvent.IsTuesday;
            model.IsWednesday = updateEvent.IsWednesday;
            model.IsThursday = updateEvent.IsThursday;
            model.IsFriday = updateEvent.IsFriday;
            model.IsSaturday = updateEvent.IsSaturday;
            model.IsSunday = updateEvent.IsSunday;
            await _context.SaveChangesAsync();
        }

        private void CheckValidateCreateEvent(CreatedEventDto newEvent)
        {
            bool isInValid = false;
            var totalDays = newEvent.EndDate.DayNumber - newEvent.StartDate.DayNumber;
            if (newEvent.StartDate == newEvent.EndDate)
            {
                isInValid = newEvent.IsMonday == true ||
                    newEvent.IsTuesday == true ||
                    newEvent.IsWednesday == true ||
                    newEvent.IsThursday == true ||
                    newEvent.IsFriday == true ||
                    newEvent.IsSaturday == true ||
                    newEvent.IsSunday == true;

                if (isInValid)
                {
                    throw new Exception("Event trong 1 ngày nên không được phép chọn repeat week");
                }
            }
            if (newEvent.StartDate != newEvent.EndDate)
            {
                isInValid = newEvent.IsMonday == null ||
                    newEvent.IsTuesday == null ||
                    newEvent.IsWednesday == null ||
                    newEvent.IsThursday == null ||
                    newEvent.IsFriday == null ||
                    newEvent.IsSaturday == null ||
                    newEvent.IsSunday == null;
                if (isInValid)
                {
                    throw new Exception("Xin chọn ít nhất 1 ngày lặp lại trong tuần");
                }
            }
            if (totalDays < 7)
            {
                var allowedDays = new HashSet<DayOfWeek>();

                var current = newEvent.StartDate;

                while (current <= newEvent.EndDate)
                {
                    allowedDays.Add(current.DayOfWeek);
                    current = current.AddDays(1);
                }

                if ((newEvent.IsMonday == true && !allowedDays.Contains(DayOfWeek.Monday)) ||
                    (newEvent.IsTuesday == true && !allowedDays.Contains(DayOfWeek.Tuesday)) ||
                    (newEvent.IsWednesday == true && !allowedDays.Contains(DayOfWeek.Wednesday)) ||
                    (newEvent.IsThursday == true && !allowedDays.Contains(DayOfWeek.Thursday)) ||
                    (newEvent.IsFriday == true && !allowedDays.Contains(DayOfWeek.Friday)) ||
                    (newEvent.IsSaturday == true && !allowedDays.Contains(DayOfWeek.Saturday)) ||
                    (newEvent.IsSunday == true && !allowedDays.Contains(DayOfWeek.Sunday)))
                {
                    throw new Exception("Repeat weekday phải nằm trong khoảng StartDate - EndDate.");
                }
            }
            if (newEvent.EndTime <= newEvent.StartTime)
            {
                throw new Exception("EndTime phải lớn hơn StartTime.");
            }
        }

        private async Task CheckOverlapEvent(CreatedEventDto newEvent)
        {
            /// <summary>
            /// Overlap khi 
            /// Event A: 08:00 - 10:00
            /// Event B: 09:00 - 11:00
            /// </summary>

            var isConflict = await _context.EventCalendars.AnyAsync(x =>
                    x.StartDate <= newEvent.EndDate &&
                    x.EndDate >= newEvent.StartDate &&
                    newEvent.StartTime < x.EndTime &&
                    newEvent.EndTime > x.StartTime &&
                    (
                        (x.IsMonday == true && newEvent.IsMonday == true) ||
                        (x.IsTuesday == true && newEvent.IsTuesday == true) ||
                        (x.IsWednesday == true && newEvent.IsWednesday == true) ||
                        (x.IsThursday == true && newEvent.IsThursday == true) ||
                        (x.IsFriday == true && newEvent.IsFriday == true) ||
                        (x.IsSaturday == true && newEvent.IsSaturday == true) ||
                        (x.IsSunday == true && newEvent.IsSunday == true)
                    )
                );

            if (isConflict)
            {
                throw new Exception("Khoảng thời gian của lịch đã bị trùng với event khác.");
            }
        }
    }
}
