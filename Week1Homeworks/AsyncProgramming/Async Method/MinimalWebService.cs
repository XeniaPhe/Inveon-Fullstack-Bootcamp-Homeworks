using System.Net;

namespace Xenia.InveonBootcamp.Homeworks.Week1.AsyncProgramming.AsyncMethod;

//A minimal locally hosted web service
internal class MinimalWebService : IDisposable, IAsyncDisposable
{
    private const ushort LowestPort = 1024;
    private const ushort HighestPort = 49151;

    private readonly HttpListener httpListener;
    private CancellationTokenSource cts;
    private Action<HttpListenerContext> requestHandler;

    internal string BaseUrl { get; init; }

    private MinimalWebService(ushort port, List<string> prefixes, Action<HttpListenerContext> requestHandler)
    {
        this.requestHandler = requestHandler;
        cts = new CancellationTokenSource();
        httpListener = new HttpListener();

        string url = $"http://localhost:{port}/";
        prefixes.ForEach(p => url += p + '/');
        BaseUrl = url;
        httpListener.Prefixes.Add(BaseUrl);
    }

    internal static MinimalWebService Create(ushort port, Action<HttpListenerContext> requestHandler)
    {
        return Create(port, [], requestHandler);
    }

    internal static MinimalWebService Create(ushort port, List<string> prefixes, Action<HttpListenerContext> requestHandler)
    {
        if (port < LowestPort || port > HighestPort)
        {
            throw new ArgumentOutOfRangeException(nameof(port), port, $"Allowed range for port numbers is [{LowestPort},{HighestPort}]");
        }

        if (requestHandler is null)
        {
            throw new ArgumentNullException(nameof(requestHandler));
        }

        if (prefixes is null)
        {
            throw new ArgumentNullException(nameof(prefixes));
        }

        return new MinimalWebService(port, prefixes, requestHandler);
    }

    internal static MinimalWebService CreateAndLaunch(ushort port, Action<HttpListenerContext> requestHandler)
    {
        var service = Create(port, requestHandler);
        _ = service.StartAsync();
        return service;
    }

    internal static MinimalWebService CreateAndLaunch(ushort port, List<string> prefixes, Action<HttpListenerContext> requestHandler)
    {
        var service = Create(port, prefixes, requestHandler);
        _ = service.StartAsync();
        return service;
    }

    internal async Task StartAsync()
    {
        if (httpListener.IsListening)
        {
            return;
        }

        httpListener.Start();

        while (!cts.IsCancellationRequested)
        {
            try
            {
                var contextTask = httpListener.GetContextAsync();
                var timeoutTask = Task.Delay(TimeSpan.FromSeconds(30), cts.Token);

                Task completedTask = await Task.WhenAny(contextTask, timeoutTask);

                if (completedTask != contextTask)
                    continue;

                if (completedTask.IsFaulted)
                {
                    foreach (var ex in completedTask.Exception.InnerExceptions)
                    {
                        Console.WriteLine($"Server Error: {ex.Message}\n");
                    }

                    continue;
                }

                //Web service itself is run on the same process it's started in
                //Each request is handled in a separate thread
                _ = Task.Run(() => requestHandler(contextTask.Result)).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        foreach (var ex in task.Exception.InnerExceptions)
                        {
                            Console.WriteLine($"Server Error: {ex.Message}\n");
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server Error: {ex.Message}\n");
            }
        }

        httpListener.Stop();
    }

    internal void Stop()
    {
        if (httpListener.IsListening)
        {
            cts.Cancel();
        }
    }

    internal async Task StopAsync()
    {
        if (httpListener.IsListening)
        {
            await cts.CancelAsync();
        }
    }

    public void Dispose()
    {
        Stop();
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();
    }
}