using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<HttpWithDI>();
services.AddHttpClient(nameof(HttpWithDI))
    .SetHandlerLifetime(TimeSpan.FromSeconds(10));
var serviceProvider = services.BuildServiceProvider();
var httpWithDi = serviceProvider.GetRequiredService<HttpWithDI>();

await httpWithDi.SendRequestToEndpoint();

public class HttpWithDI
{
    private IHttpClientFactory _httpClientFactory;

    public HttpWithDI(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task SendRequestToEndpoint()
    {
        for (var count = 0; count < 100_000_000; count++)
        {
            var httpClient = _httpClientFactory.CreateClient(nameof(HttpWithDI));
            await httpClient.GetStringAsync("http://mygreatapi.internal:5119/weatherforecast");
            Console.WriteLine("Total fired requests: {0}", count);
        }
    }
}