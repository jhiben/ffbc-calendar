using FFBC.Models;

namespace FFBC.Services;

public class InMemoryEventStore : IEventStore
{
    private readonly List<Event> _events;

    public InMemoryEventStore(IEnumerable<Event> seed)
    {
        _events = new List<Event>(seed);
    }

    public IReadOnlyList<Event> GetAll() => _events.AsReadOnly();
}
