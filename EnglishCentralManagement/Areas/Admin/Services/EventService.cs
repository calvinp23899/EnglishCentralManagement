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
            var model = await _context.EventCalendars
                .Where(x => x.Id == updateEvent.EventId && !x.IsDeleted)
                .FirstOrDefaultAsync();
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
    }
}
