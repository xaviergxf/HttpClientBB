using System.Net.NetworkInformation;

var exitEvent = new CancellationTokenSource();
const int remoteEndpointPort = 5119;

Console.CancelKeyPress += (sender, eventArgs) =>
{
    eventArgs.Cancel = true;
    exitEvent.Cancel();
};

IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

var (cursorLeftPos, cursorTopPos) = Console.GetCursorPosition();
try
{
    while (!exitEvent.IsCancellationRequested)
    {
        Console.Clear();
        //Console.SetCursorPosition(0, cursorTopPos);
        var totalActiveConnections = properties.GetActiveTcpConnections()
            .Where(p => p.RemoteEndPoint.Port == remoteEndpointPort).Count();
        Console.WriteLine("Total connections for port {0}: {1}", remoteEndpointPort, totalActiveConnections);
        await Task.Delay(200, exitEvent.Token);
    }
}
catch (TaskCanceledException)
{ }
