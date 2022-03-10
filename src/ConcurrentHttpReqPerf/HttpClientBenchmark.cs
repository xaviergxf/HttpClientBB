using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Microsoft.Extensions.DependencyInjection;
using System.Net.NetworkInformation;

namespace ConcurrentHttpReqPerf
{
    [SimpleJob(RunStrategy.Monitoring, targetCount: 1)]
    public class HttpClientBenchmark
    {
        private const int _remoteEndpointPort = 5119;
        private const string _httpClientName = "myHttpClient";
        private ServiceProvider? _serviceProvider;
        private IHttpClientFactory? _httpClientFactory;
        private readonly int[] _requestsList;
        private const int _numberOfRequests = 2000;

        public HttpClientBenchmark()
        {
            _requestsList = Enumerable.Range(0, _numberOfRequests).ToArray();
        }

        [IterationSetup]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddHttpClient(_httpClientName);

            _serviceProvider = services.BuildServiceProvider();
            _httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        }

        [IterationCleanup]
        public void Cleanup()
        {
            _serviceProvider?.Dispose();
            Console.WriteLine("Cleaning http clients...");
            Thread.Sleep(TimeSpan.FromMinutes(4));
            Console.WriteLine("Http clients cleaned");

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            var totalActiveConnections = properties.GetActiveTcpConnections()
                .Where(p => p.RemoteEndPoint.Port == _remoteEndpointPort).Count();
            Console.WriteLine("Total connections for port {0}: {1}", _remoteEndpointPort, totalActiveConnections);
        }

        [Benchmark]
        public async Task WithDI()
        {
            await Parallel.ForEachAsync(_requestsList, async (s, c) =>
            {
                var httpClient = _httpClientFactory!.CreateClient(_httpClientName);
                await httpClient.GetAsync($"http://mygreatapi.internal:{_remoteEndpointPort}/weatherforecast");
            });
        }

        [Benchmark]
        public async Task WithDispose()
        {
            await Parallel.ForEachAsync(_requestsList, async (s, c) =>
            {
                using var httpClient = new HttpClient();
                await httpClient.GetAsync($"http://mygreatapi.internal:{_remoteEndpointPort}/weatherforecast");
            });
        }

        [Benchmark]
        public async Task WithStatic()
        {
            var staticHttpClient = new HttpClient();
            await Parallel.ForEachAsync(_requestsList, async (s, c) =>
            {
                await staticHttpClient!.GetAsync($"http://mygreatapi.internal:{_remoteEndpointPort}/weatherforecast");
            });
        }
    }
}
