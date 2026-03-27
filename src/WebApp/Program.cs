using FFBC.Models;
using FFBC.Options;
using FFBC.Services;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient("ffbc");
builder.Services.AddHttpClient("nominatim", client =>
{
    client.DefaultRequestHeaders.UserAgent.ParseAdd("FFBC-WebApp/1.0 (mountain-biking-events)");
});
builder.Services.Configure<FfbcEventStoreOptions>(builder.Configuration.GetSection(FfbcEventStoreOptions.SectionName));
builder.Services.AddSingleton<IEventStore, FfbcWebEventStore>();
builder.Services.AddSingleton<IGeocodingService, NominatimGeocodingService>();
builder.Services.AddSingleton<IEventDetailsService, FfbcEventDetailsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapGet("/feed.xml", (IEventStore eventStore, HttpContext ctx) =>
{
    var request = ctx.Request;
    var baseUrl = $"{request.Scheme}://{request.Host}";

    var feed = new SyndicationFeed(
        "FFBC Calendar Events",
        "Upcoming FFBC-licensed mountain bike events",
        new Uri(baseUrl));

    var items = eventStore.GetAll()
        .OrderBy(e => e.Date)
        .Select(e =>
        {
            var detailUrl = $"{baseUrl}/EventDetail?date={e.Date:yyyy-MM-dd}&title={Uri.EscapeDataString(e.Title)}";
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
});

app.Run();
