using FFBC.Models;

namespace FFBC.Services;

public interface IEventDetailsService
{
    Task<EventDetails?> GetEventDetailsAsync(string eventId);
}
