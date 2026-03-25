using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FFBC.Models;
using FFBC.Options;
using FFBC.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace FFBC.Tests;

public class FfbcWebEventStoreTests
{
    [Fact]
    public void GetAll_ReturnsCachedResultWithoutSecondHttpCall()
    {
        var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(BuildHtmlRow("Rallye des Spirous", "Mercredi 25 Mars 2026", "Namur - 5190 Spy", string.Empty), Encoding.UTF8, "text/html")
            });
        var store = BuildStore(handler);

        var firstResult = store.GetAll();
        var secondResult = store.GetAll();

        Assert.Single(firstResult);
        Assert.Single(secondResult);
        Assert.Equal(1, handler.CallCount);
    }

    [Fact]
    public void GetAll_ParsesSuccessfulResponseAndStoresItInCache()
    {
        var cache = new MemoryCache(new MemoryCacheOptions());
        var handler = new StubHttpMessageHandler(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(BuildHtmlRow("La Florancy", "Samedi 25 Avril 2026", "Luxembourg - 6780 MESSANCY", "Brevet à Dénivelé"), Encoding.UTF8, "text/html")
            });
        var options = CreateOptions();
        var store = BuildStore(handler, cache, options);

        var result = store.GetAll();

        Assert.Single(result);
        Assert.Equal("La Florancy", result[0].Title);
        Assert.Equal(new DateTime(2026, 4, 25), result[0].Date);
        Assert.Contains("Luxembourg - 6780 MESSANCY", result[0].Notes);
        Assert.Equal("6780", result[0].PostalCode);
        Assert.Equal("MESSANCY", result[0].Town);
        Assert.Equal("Belgium", result[0].Country);
        Assert.True(cache.TryGetValue(FfbcWebEventStore.BuildCacheKey(options), out IReadOnlyList<Event>? cached));
        Assert.NotNull(cached);
        Assert.Single(cached!);
    }

    [Fact]
    public void GetAll_ReturnsEmptyListWhenHttpRequestFails_AndDoesNotCacheFailure()
    {
        var cache = new MemoryCache(new MemoryCacheOptions());
        var handler = new StubHttpMessageHandler(_ => throw new HttpRequestException("boom"));
        var options = CreateOptions();
        var store = BuildStore(handler, cache, options);

        var result = store.GetAll();

        Assert.Empty(result);
        Assert.False(cache.TryGetValue(FfbcWebEventStore.BuildCacheKey(options), out _));
    }

    [Fact]
    public void BuildPayload_ContainsConfiguredFormDataFields()
    {
        var options = CreateOptions();

        var payload = FfbcWebEventStore.BuildPayload(options);

        Assert.Equal("1", payload["ajax"]);
        Assert.Equal(options.BaseUrl, payload["baseUrl"]);
        Assert.Equal(options.Year.ToString(), payload["year_selected"]);
        Assert.Equal(options.Challenge, payload["challenge"]);
        Assert.Equal(options.VttRoute, payload["vtt_route"]);
        Assert.Equal(options.Province, payload["province"]);
        Assert.Equal(options.Date, payload["date"]);
    }

    [Fact]
    public void GetAll_SendsConfiguredFormDataPayload()
    {
        var handler = new StubHttpMessageHandler(request =>
        {
            var body = request.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
            Assert.Contains("ajax=1", body);
            Assert.Contains("year_selected=2026", body);
            Assert.Contains("province=Luxembourg", body);
            Assert.Contains("challenge=", body);
            Assert.Contains("vtt_route=vtt%2Fmarche", body);
            Assert.Contains("date=2026-03-25", body);
            Assert.True(request.Headers.TryGetValues("X-Requested-With", out var values));
            Assert.Contains("XMLHttpRequest", values);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(BuildHtmlRow("Rallye des Spirous", "Mercredi 25 Mars 2026", "Namur - 5190 Spy", string.Empty), Encoding.UTF8, "text/html")
            };
        });
        var store = BuildStore(handler);

        var result = store.GetAll();

        Assert.Single(result);
    }

    [Fact]
    public void TryParseEvents_ExtractsPostalCodeAndTown_WhenPlaceHasBelgianFormat()
    {
        var html = BuildHtmlRow("Test Ride", "Mercredi 25 Mars 2026", "Namur - 5190 Spy", "");

        var result = FfbcWebEventStore.TryParseEvents(html, out var events);

        Assert.True(result);
        Assert.Single(events);
        Assert.Equal("5190", events[0].PostalCode);
        Assert.Equal("Spy", events[0].Town);
        Assert.Equal("Belgium", events[0].Country);
    }

    [Fact]
    public void TryParseEvents_SetsPostalCodeNull_WhenPlaceHasNoPostalCode()
    {
        var html = BuildHtmlRow("Test Ride", "Mercredi 25 Mars 2026", "Some Place", "");

        var result = FfbcWebEventStore.TryParseEvents(html, out var events);

        Assert.True(result);
        Assert.Single(events);
        Assert.Null(events[0].PostalCode);
        Assert.Equal("Some Place", events[0].Town);
        Assert.Null(events[0].Country);
    }

    [Fact]
    public void TryParseEvents_SetsTownAndPostalCodeNull_WhenPlaceIsEmpty()
    {
        var html = BuildHtmlRow("Test Ride", "Mercredi 25 Mars 2026", "", "Challenge");

        var result = FfbcWebEventStore.TryParseEvents(html, out var events);

        Assert.True(result);
        Assert.Single(events);
        Assert.Null(events[0].PostalCode);
        Assert.Null(events[0].Town);
        Assert.Null(events[0].Country);
    }

    [Fact]
    public void TryParseEvents_ExtractsPostalCodeAndTown_WhenPlaceHasProvincePrefix()
    {
        var html = BuildHtmlRow("Test Ride", "Mercredi 25 Mars 2026", "Luxembourg - 6780 MESSANCY", "");

        var result = FfbcWebEventStore.TryParseEvents(html, out var events);

        Assert.True(result);
        Assert.Single(events);
        Assert.Equal("6780", events[0].PostalCode);
        Assert.Equal("MESSANCY", events[0].Town);
        Assert.Equal("Belgium", events[0].Country);
    }

    [Fact]
    public void TryParseEvents_ExtractsPostalCodeTownAndCountry_WhenPlaceHasBelgianPrefix()
    {
        var html = BuildHtmlRow("Test Ride", "Mercredi 25 Mars 2026", "B-1234 Bruxelles", "");

        var result = FfbcWebEventStore.TryParseEvents(html, out var events);

        Assert.True(result);
        Assert.Single(events);
        Assert.Equal("1234", events[0].PostalCode);
        Assert.Equal("Bruxelles", events[0].Town);
        Assert.Equal("Belgium", events[0].Country);
    }

    [Fact]
    public void TryParseEvents_ExtractsPostalCodeTownAndCountry_WhenPlaceHasFrenchPrefix()
    {
        var html = BuildHtmlRow("French Ride", "Mercredi 25 Mars 2026", "F-59000 Lille", "");

        var result = FfbcWebEventStore.TryParseEvents(html, out var events);

        Assert.True(result);
        Assert.Single(events);
        Assert.Equal("59000", events[0].PostalCode);
        Assert.Equal("Lille", events[0].Town);
        Assert.Equal("France", events[0].Country);
    }

    [Fact]
    public void TryParseEvents_ExtractsPostalCodeTownAndCountry_WhenPlaceHasBareFrenchPostalCode()
    {
        var html = BuildHtmlRow("Bare French Ride", "Mercredi 25 Mars 2026", "59000 Lille", "");

        var result = FfbcWebEventStore.TryParseEvents(html, out var events);

        Assert.True(result);
        Assert.Single(events);
        Assert.Equal("59000", events[0].PostalCode);
        Assert.Equal("Lille", events[0].Town);
        Assert.Equal("France", events[0].Country);
    }

    private static FfbcWebEventStore BuildStore(
        StubHttpMessageHandler handler,
        IMemoryCache? cache = null,
        FfbcEventStoreOptions? options = null)
    {
        var httpClient = new HttpClient(handler);
        var httpClientFactory = new StubHttpClientFactory(httpClient);

        return new FfbcWebEventStore(
            httpClientFactory,
            cache ?? new MemoryCache(new MemoryCacheOptions()),
            Microsoft.Extensions.Options.Options.Create(options ?? CreateOptions()),
            NullLogger<FfbcWebEventStore>.Instance);
    }

    private static FfbcEventStoreOptions CreateOptions() => new()
    {
        PostUrl = "https://www.velo-liberte.be/wp-content/themes/zapedah-child/assets/scripts/ajax_request_calendrier.php",
        BaseUrl = "https://www.velo-liberte.be/wp-content/themes/zapedah-child",
        Province = "Luxembourg",
        Challenge = string.Empty,
        VttRoute = "vtt/marche",
        Year = 2026,
        Date = "2026-03-25",
        CacheDurationMinutes = 120
    };

    private static string BuildHtmlRow(string title, string date, string place, string challenge) =>
        $"<div class=\"calendar-row\"><div class=\"calendar-row-informations\"><div class=\"calendar-row-name\"><b>{title}</b></div><div class=\"calendar-row-date\">{date}</div><div class=\"calendar-row-place\">{place}</div><div class=\"calendar-row-challenge\">{challenge}</div></div></div>";

    private sealed class StubHttpClientFactory(HttpClient client) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => client;
    }

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responder) : HttpMessageHandler
    {
        public int CallCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CallCount++;
            return Task.FromResult(responder(request));
        }
    }
}