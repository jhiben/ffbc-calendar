using FFBC.Models;

namespace FFBC.Services;

public interface IEventStore
{
    IReadOnlyList<Event> GetAll();
    Event? GetByDateAndTitle(DateTime date, string title);
}
