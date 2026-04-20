using System.Text.Json;
using FFBC.Functions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FFBC.Tests;

public class GeocodeFunctionTests
{
    [Fact]
    public async Task Run_ReturnsCoordinates_WhenFound()
    {
        var service = new StubGeocodingService((50.4669, 4.8719));
        var function = new GeocodeFunction(service);
        var httpContext = new DefaultHttpContext();

        var result = await function.Run(httpContext.Request, "5000");

        var (status, body) = await ExecuteResult(result);
        Assert.Equal(200, status);

        var json = JsonSerializer.Deserialize<JsonElement>(body);
        Assert.Equal(50.4669, json.GetProperty("latitude").GetDouble(), 4);
        Assert.Equal(4.8719, json.GetProperty("longitude").GetDouble(), 4);
    }

    [Fact]
    public async Task Run_ReturnsNotFound_WhenNull()
    {
        var service = new StubGeocodingService(null);
        var function = new GeocodeFunction(service);
        var httpContext = new DefaultHttpContext();

        var result = await function.Run(httpContext.Request, "9999");

        var (status, _) = await ExecuteResult(result);
        Assert.Equal(404, status);
    }

    [Fact]
    public async Task Run_ReturnsNotFound_WhenPostalCodeIsEmpty()
    {
        var service = new StubGeocodingService(null);
        var function = new GeocodeFunction(service);
        var httpContext = new DefaultHttpContext();

        var result = await function.Run(httpContext.Request, "  ");

        var (status, _) = await ExecuteResult(result);
        Assert.Equal(404, status);
    }

    private static async Task<(int StatusCode, string Body)> ExecuteResult(IResult result)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider();
        httpContext.Response.Body = new MemoryStream();
        await result.ExecuteAsync(httpContext);
        httpContext.Response.Body.Position = 0;
        var body = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
        return (httpContext.Response.StatusCode, body);
    }
}
