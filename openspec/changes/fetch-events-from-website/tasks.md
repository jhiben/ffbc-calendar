## 1. NuGet Dependencies

- [x] 1.1 Add `HtmlAgilityPack` NuGet package to `src/WebApp/WebApp.csproj`

## 2. Configuration

- [x] 2.1 Add `FfbcEventStoreOptions` POCO class with properties: `PostUrl`, `BaseUrl`, `Province`, `Challenge`, `Year`, `Date`, `CacheDurationMinutes`
- [x] 2.2 Add `FfbcEventStore` configuration section to `appsettings.json` with default values (province: Luxembourg, challenge: vtt/marche, cacheDurationMinutes: 120)

## 3. FfbcWebEventStore Implementation

- [x] 3.1 Create `FfbcWebEventStore : IEventStore` class accepting `IHttpClientFactory`, `IMemoryCache`, and `IOptions<FfbcEventStoreOptions>` in its constructor
- [x] 3.2 Implement `GetAll()`: check `IMemoryCache` for cached result and return it if present
- [x] 3.3 If cache miss, build the POST form-data payload (`ajax=1`, `baseUrl`, `year_selected`, `challenge`, `province`, `date`) and send via named `HttpClient`
- [x] 3.4 Parse the HTML response using `HtmlAgilityPack` to extract event date, title, and notes from the response fragment
- [x] 3.5 On successful parse, store result in `IMemoryCache` with absolute expiry equal to configured TTL and return it
- [x] 3.6 On `HttpRequestException` or parse failure, log a warning and return an empty list (do not cache)

## 4. DI Registration

- [x] 4.1 Register `IMemoryCache` via `builder.Services.AddMemoryCache()` in `Program.cs`
- [x] 4.2 Register named `HttpClient` `"ffbc"` via `builder.Services.AddHttpClient("ffbc")` in `Program.cs`
- [x] 4.3 Bind `FfbcEventStoreOptions` from configuration and register `FfbcWebEventStore` as the `IEventStore` singleton, replacing `InMemoryEventStore`

## 5. Tests

- [x] 5.1 Write a unit test verifying that a cached result is returned without an HTTP call when the cache is populated
- [x] 5.2 Write a unit test verifying that a successful HTTP response is parsed into events and stored in the cache
- [x] 5.3 Write a unit test verifying that an `HttpRequestException` results in an empty list being returned and nothing being written to the cache
- [x] 5.4 Write a unit test verifying that the form-data payload contains the correct fields from configuration
