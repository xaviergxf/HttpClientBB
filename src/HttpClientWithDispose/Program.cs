var approach = new HttpClientWithDisposeApproach();
await approach.SendRequestToEndpoint();

public class HttpClientWithDisposeApproach
{
    public async Task SendRequestToEndpoint()
    {
        for (var count = 0; count < 100_000_000; count++)
        {
            using (var client = new HttpClient())
            {
                await client.GetStringAsync("http://mygreatapi.internal:5119/weatherforecast");
                Console.WriteLine("Total fired requests: {0}", count);
            }
        }
    }
}

