using System.Net;
using System.Text;
using Xenia.InveonBootcamp.Homeworks.Week1.AsyncProgramming.AsyncMethod;

namespace Xenia.InveonBootcamp.Homeworks.Week1.AsyncProgramming.AsyncAwaitUsage;
internal class PIApproximationService : IDisposable, IAsyncDisposable
{
    private MinimalWebService api;

    internal PIApproximationService()
    {
        this.api = MinimalWebService.Create(5000, ["api", "pi"], WebService);
    }

    internal string GetUrlFor(int k) => $"{api.BaseUrl}{k}/";
    internal void Launch() => _ = api.StartAsync();
    internal void Stop() => api.Stop();
    internal async Task StopAsync() => await api.StopAsync();
    public void Dispose() => Stop();
    public async ValueTask DisposeAsync() => await StopAsync();

    internal static PIApproximationService CreateAndLaunch()
    {
        var service = new PIApproximationService();
        service.Launch();
        return service;
    }

    private static void WebService(HttpListenerContext context)
    {
        void CloseResponse(HttpStatusCode httpStatusCode)
        {
            context.Response.StatusCode = (int)httpStatusCode;
            context.Response.Close();
        }

        string path = context.Request.Url!.AbsolutePath;

        if (!path.EndsWith("/"))
        {
            path += "/";
        }

        var pathSegments = path.Split('/');
        int k;

        if (pathSegments.Length < 5 || (k = Convert.ToInt32(pathSegments[3])) < 1)
        {
            CloseResponse(HttpStatusCode.BadRequest);
            return;
        }

        var approximations = new List<double>(k);
        foreach (int n in Enumerable.Range(3, k))
        {
            approximations.Add(ApproximatePIWithPolygon(n));
        }

        context.Response.ContentType = "text/plain";
        context.Response.AddHeader("Content-Disposition", "attachment; filename=pi_approximations.txt"); // Suggested filename for download

        // 16 bytes per approximation = integer part (1) + decimal point (1) + digits after decimal point (13) + new line (1)
        StringBuilder sb = new StringBuilder(k * 16);
        foreach (var approximation in approximations)
        {
            sb.Append($"{approximation.ToString("F13")}\n");
        }

        string textContent = sb.ToString();

        context.Response.ContentLength64 = Encoding.UTF8.GetByteCount(textContent);
        using (var writer = new StreamWriter(context.Response.OutputStream))
        {
            writer.Write(textContent);
        }

        CloseResponse(HttpStatusCode.OK);
    }

    //Requires prior knowledge of PI but who cares
    private static double ApproximatePIWithPolygon(int n)
    {
        double unequalAngle = 2 * Math.PI / n;
        double equalAngle = (Math.PI - unequalAngle) / 2.0;
        double unequalSide = Math.Sin(unequalAngle) / Math.Sin(equalAngle);
        double perimeter = unequalSide * n;
        return perimeter / 2.0;
    }
}