using System.Net;
using System.Text;
using FFBC.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace FFBC.Tests;

public class FfbcEventDetailsServiceTests
{
    [Fact]
    public async Task GetEventDetailsAsync_ReturnsNull_WhenEventIdIsNullOrEmpty()
    {
        var service = BuildService(_ => new HttpResponseMessage(HttpStatusCode.OK));

        var resultNull = await service.GetEventDetailsAsync(null!);
        var resultEmpty = await service.GetEventDetailsAsync("");
        var resultWhitespace = await service.GetEventDetailsAsync("   ");

        Assert.Null(resultNull);
        Assert.Null(resultEmpty);
        Assert.Null(resultWhitespace);
    }

    [Fact]
    public async Task GetEventDetailsAsync_ReturnsCachedResult_OnSubsequentCalls()
    {
        var callCount = 0;
        var service = BuildService(_ =>
        {
            callCount++;
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("<div>Test content</div>", Encoding.UTF8, "text/html")
            };
        });

        var result1 = await service.GetEventDetailsAsync("123");
        var result2 = await service.GetEventDetailsAsync("123");

        Assert.NotNull(result1);
        Assert.NotNull(result2);
        Assert.Equal(1, callCount);
    }

    [Fact]
    public async Task GetEventDetailsAsync_ReturnsNull_WhenApiReturnsError()
    {
        var service = BuildService(_ => new HttpResponseMessage(HttpStatusCode.InternalServerError));

        var result = await service.GetEventDetailsAsync("123");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetEventDetailsAsync_ReturnsNull_WhenApiReturnsEmptyContent()
    {
        var service = BuildService(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("", Encoding.UTF8, "text/html")
        });

        var result = await service.GetEventDetailsAsync("123");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetEventDetailsAsync_ReturnsNull_WhenHttpRequestFails()
    {
        var service = BuildService(_ => throw new HttpRequestException("Network error"));

        var result = await service.GetEventDetailsAsync("123");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetEventDetailsAsync_ParsesHtmlContent_AndSetsEventId()
    {
        var html = BuildPopupHtml();
        var service = BuildService(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(html, Encoding.UTF8, "text/html")
        });

        var result = await service.GetEventDetailsAsync("456");

        Assert.NotNull(result);
        Assert.Equal("456", result!.EventId);
        Assert.Equal(html, result.HtmlContent);
    }

    [Fact]
    public void ParseEventDetails_ExtractsRegistrationUrl()
    {
        var html = BuildPopupHtml();

        var result = FfbcEventDetailsService.ParseEventDetails("123", html);

        Assert.NotNull(result);
        Assert.Equal("https://velo-liberte-palmares.be/inscription/1004945/", result!.RegistrationUrl);
    }

    [Fact]
    public void ParseEventDetails_ExtractsStartLocationAndAddress()
    {
        var html = BuildPopupHtml();

        var result = FfbcEventDetailsService.ParseEventDetails("123", html);

        Assert.NotNull(result);
        Assert.Equal("Syndicat d'Initiative 'Im Wohr'", result!.StartLocation);
        Assert.Equal("43 Rue de Radelange 6630 MARTELANGE", result!.Address);
        Assert.Equal("https://www.google.com/maps/search/?api=1&query=43 Rue de Radelange 6630 MARTELANGE", result!.MapsUrl);
    }

    [Fact]
    public void ParseEventDetails_ExtractsClub()
    {
        var html = BuildPopupHtml();

        var result = FfbcEventDetailsService.ParseEventDetails("123", html);

        Assert.NotNull(result);
        Assert.Equal("2407 - BIKERS ARDENNAIS", result!.Club);
    }

    [Fact]
    public void ParseEventDetails_ExtractsActivities()
    {
        var html = BuildPopupHtml();

        var result = FfbcEventDetailsService.ParseEventDetails("123", html);

        Assert.NotNull(result);
        Assert.Equal(2, result!.Activities.Count);

        var first = result.Activities[0];
        Assert.Equal("VTT", first.Type);
        Assert.Equal("55 Km", first.Distance);
        Assert.Equal("450m", first.Elevation);
        Assert.Equal("07h00 - 10h00", first.Time);
        Assert.Equal("2", first.Ravito);
        Assert.Equal("FFBC : 5,50€ / NON FFBC : 7,00€", first.Price);

        var second = result.Activities[1];
        Assert.Equal("MARCHE", second.Type);
        Assert.Equal("10 Km", second.Distance);
        Assert.Equal("N.A", second.Elevation);
        Assert.Equal("07h00 - 11h00", second.Time);
        Assert.Null(second.Price);
    }

    [Fact]
    public void ParseEventDetails_ExtractsNotes()
    {
        var html = BuildPopupHtml();

        var result = FfbcEventDetailsService.ParseEventDetails("123", html);

        Assert.NotNull(result);
        Assert.Contains("Douches", result!.Notes);
    }

    [Fact]
    public void ParseEventDetails_ExtractsWebsite()
    {
        var html = BuildPopupHtml();

        var result = FfbcEventDetailsService.ParseEventDetails("123", html);

        Assert.NotNull(result);
        Assert.Equal("http://www.example.com/event", result!.Website);
    }

    [Fact]
    public void ParseEventDetails_ReturnsEmptyActivities_WhenNoActivitiesPresent()
    {
        var html = "<div class='calendar-details'><div class='calendar-details-club'><b>Club organisateur :</b><br> Test Club</div></div>";

        var result = FfbcEventDetailsService.ParseEventDetails("123", html);

        Assert.NotNull(result);
        Assert.Empty(result!.Activities);
        Assert.Equal("Test Club", result.Club);
    }

    [Fact]
    public void ParseEventDetails_HandlesMinimalHtml()
    {
        var html = "<div>No structured data here</div>";

        var result = FfbcEventDetailsService.ParseEventDetails("123", html);

        Assert.NotNull(result);
        Assert.Null(result!.StartLocation);
        Assert.Null(result!.Club);
        Assert.Empty(result!.Activities);
    }

    [Fact]
    public void StripLabel_RemovesPrefixLabel()
    {
        Assert.Equal("VTT", FfbcEventDetailsService.StripLabel("Type : VTT", "Type :"));
        Assert.Equal("2407 - BIKERS", FfbcEventDetailsService.StripLabel("Club organisateur : 2407 - BIKERS", "Club organisateur :"));
        Assert.Null(FfbcEventDetailsService.StripLabel("Type :", "Type :"));
        Assert.Null(FfbcEventDetailsService.StripLabel(null, "Type :"));
    }

    private static FfbcEventDetailsService BuildService(Func<HttpRequestMessage, HttpResponseMessage> responder)
    {
        var handler = new StubHttpMessageHandler(responder);
        var httpClient = new HttpClient(handler);
        var httpClientFactory = new StubHttpClientFactory(httpClient);
        var cache = new MemoryCache(new MemoryCacheOptions());

        return new FfbcEventDetailsService(
            httpClientFactory,
            cache,
            NullLogger<FfbcEventDetailsService>.Instance);
    }

    private static string BuildPopupHtml() =>
        """
        <div class="calendar-details-subscription">
            <a href="https://velo-liberte-palmares.be/inscription/1004945/" target="_blank" class="calendar-activities-preregister">Je m'inscris</a>
        </div>
        <div class="calendar-details">
            <div class="calendar-details-place">
                <b> Lieu de d&eacute;part : </b><br> Syndicat d'Initiative 'Im Wohr' <br>
                43 Rue de Radelange 6630 MARTELANGE <br>
                <a href="https://www.google.com/maps/search/?api=1&amp;query=43 Rue de Radelange 6630 MARTELANGE" target=_blank>
                    <img src="/assets/images/navigation.svg"> Itin&eacute;raire
                </a>
            </div>
            <div class="calendar-details-club"><b> Club organisateur :</b><br> 2407 - BIKERS ARDENNAIS</div>
            <div class="calendar-details-activities">
                <div class='calendar-activities-row'>
                    <div class='calendar-activities-row-type'><b>Type :</b> VTT</div>
                    <div class='calendar-activities-row-km'><img src='/assets/images/distance.svg'>55 Km</div>
                    <div class='calendar-activities-row-denivele'><b>D+ :</b> 450m</div>
                    <div class='calendar-activities-row-dificulty'><img src='/assets/images/velo.svg'> Moderate</div>
                    <div class='calendar-activities-row-date'><img src='/assets/images/clock.svg'>07h00 - 10h00</div>
                    <div class='calendar-activities-row-ravito'><b>Ravito : </b>2</div>
                    <div class='calendar-activities-row-price-ffbc'><b>Tarif :</b>  FFBC : 5,50€ / NON FFBC : 7,00€ </div>
                </div>
                <div class='calendar-activities-row'>
                    <div class='calendar-activities-row-type'><b>Type :</b> MARCHE</div>
                    <div class='calendar-activities-row-km'><img src='/assets/images/distance.svg'>10 Km</div>
                    <div class='calendar-activities-row-denivele'><b>D+ :</b> N.A</div>
                    <div class='calendar-activities-row-dificulty'><img src='/assets/images/velo.svg'> N.A</div>
                    <div class='calendar-activities-row-date'><img src='/assets/images/clock.svg'>07h00 - 11h00</div>
                    <div class='calendar-activities-row-ravito'><b>Ravito : </b>N.A</div>
                </div>
            </div>
            <div class="calendar-details-note"><b> Remarque :</b> Douches H/F - vestiaires - Bike Wash</div>
            <div class="calendar-details-website">
                <img src="/assets/images/worldwide.svg">
                <a target=_blank href="http://www.example.com/event">www.example.com/event</a>
            </div>
        </div>
        """;

    private sealed class StubHttpClientFactory(HttpClient client) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => client;
    }

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responder) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(responder(request));
        }
    }
}
