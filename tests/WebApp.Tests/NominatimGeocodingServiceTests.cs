using System.Net;
using System.Text;
using FFBC.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace FFBC.Tests;

public class NominatimGeocodingServiceTests
{
    [Fact]
    public async Task GeocodeAsync_ReturnsCoordinates_WhenNominatimReturnsResults()
    {
        var json = """[{"lat":"50.4669","lon":"4.8719"}]""";
        var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });

        var service = BuildService(handler);

        var result = await service.GeocodeAsync("5000", "Namur");

        Assert.NotNull(result);
        Assert.Equal(50.4669, result.Value.Latitude, 4);
        Assert.Equal(4.8719, result.Value.Longitude, 4);
    }

    [Fact]
    public async Task GeocodeAsync_ReturnsNull_WhenNominatimReturnsEmptyArray()
    {
        var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("[]", Encoding.UTF8, "application/json")
        });

        var service = BuildService(handler);

        var result = await service.GeocodeAsync("9999", null);

        Assert.Null(result);
    }

    [Fact]
    public async Task GeocodeAsync_ReturnsNull_WhenHttpRequestFails()
    {
        var handler = new StubHandler(_ => throw new HttpRequestException("timeout"));

        var service = BuildService(handler);

        var result = await service.GeocodeAsync("5000", "Namur");

        Assert.Null(result);
    }

    [Fact]
    public async Task GeocodeAsync_ReturnsCachedResult_OnSecondCall()
    {
        var json = """[{"lat":"50.4669","lon":"4.8719"}]""";
        var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });

        var service = BuildService(handler);

        var first = await service.GeocodeAsync("5000", "Namur");
        var second = await service.GeocodeAsync("5000", "Namur");

        Assert.NotNull(first);
        Assert.NotNull(second);
        Assert.Equal(first, second);
        Assert.Equal(1, handler.CallCount);
    }

    [Fact]
    public async Task GeocodeAsync_CachesNullResult_SoFailedLookupIsNotRetried()
    {
        var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("[]", Encoding.UTF8, "application/json")
        });

        var service = BuildService(handler);

        var first = await service.GeocodeAsync("9999", null);
        var second = await service.GeocodeAsync("9999", null);

        Assert.Null(first);
        Assert.Null(second);
        Assert.Equal(1, handler.CallCount);
    }

    private static NominatimGeocodingService BuildService(StubHandler handler)
    {
        var httpClient = new HttpClient(handler);
        var factory = new StubFactory(httpClient);
        var cache = new MemoryCache(new MemoryCacheOptions());
        return new NominatimGeocodingService(factory, cache, NullLogger<NominatimGeocodingService>.Instance);
    }

    private sealed class StubFactory(HttpClient client) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => client;
    }

    private sealed class StubHandler(Func<HttpRequestMessage, HttpResponseMessage> responder) : HttpMessageHandler
    {
        public int CallCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CallCount++;
            return Task.FromResult(responder(request));
        }
    }
}
