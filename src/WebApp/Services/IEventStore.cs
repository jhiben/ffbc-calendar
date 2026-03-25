using FFBC.Models;

namespace FFBC.Services;

public interface IEventStore
{
    IReadOnlyList<Event> GetAll();
}
