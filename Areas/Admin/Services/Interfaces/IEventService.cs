using EnglishCentralManagement.Dtos;

namespace EnglishCentralManagement.Areas.Admin.Services.Interfaces
{
    public interface IEventService
    {
        Task CreateEvent(CreatedEventDto newEvent);
        Task UpdateEvent(CreatedEventDto updateEvent);
        Task DeleteEvent(long id);
        Task<List<CreatedEventDto>> GetAllEventById(long id);
    }
}
