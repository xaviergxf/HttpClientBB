var approach = new HttpClientWithStaticApproach();
await approach.SendRequestToEndpoint();

public class HttpClientWithStaticApproach
{
    private static readonly HttpClient _httpClient = new();
    public async Task SendRequestToEndpoint()
    {
        for (var count = 0; count < 100_000_000; count++)
        {
            await _httpClient.GetStringAsync("http://mygreatapi.internal:5119/weatherforecast");
            Console.WriteLine("Total fired requests: {0}", count);
        }
    }
}

