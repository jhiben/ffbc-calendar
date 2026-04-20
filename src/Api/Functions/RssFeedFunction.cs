using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using FFBC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;

namespace FFBC.Functions;

public class RssFeedFunction
{
    private readonly IEventStore _eventStore;

    public RssFeedFunction(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    [Function("RssFeed")]
    public IResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "feed.xml")] HttpRequest req)
    {
        var baseUrl = $"{req.Scheme}://{req.Host}";

        var feed = new SyndicationFeed(
            "FFBC Calendar Events",
            "Upcoming FFBC-licensed mountain bike events",
            new Uri(baseUrl));

        var items = _eventStore.GetAll()
            .OrderBy(e => e.Date)
            .Select(e =>
            {
                var detailUrl = $"{baseUrl}/event?date={e.Date:yyyy-MM-dd}&title={Uri.EscapeDataString(e.Title)}";
                var description = string.Join(" — ",
                    new[] { e.Town, e.Country, e.Notes }.Where(s => !string.IsNullOrWhiteSpace(s)));

                return new SyndicationItem(
                    e.Title,
                    description,
                    new Uri(detailUrl))
                {
                    PublishDate = new DateTimeOffset(e.Date, TimeSpan.Zero),
                    Id = detailUrl
                };
            })
            .ToList();

        feed.Items = items;
        feed.LastUpdatedTime = DateTimeOffset.UtcNow;

        using var stream = new MemoryStream();
        using (var writer = XmlWriter.Create(stream, new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            Indent = true
        }))
        {
            new Rss20FeedFormatter(feed).WriteTo(writer);
        }

        return Results.Content(
            Encoding.UTF8.GetString(stream.ToArray()),
            "application/rss+xml",
            Encoding.UTF8);
    }
}
