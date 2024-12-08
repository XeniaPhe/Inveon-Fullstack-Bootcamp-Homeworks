namespace Xenia.InveonBootcamp.Homeworks.Week1.AsyncProgramming.AsyncAwaitUsage;
internal static class FileDownloader
{
    internal static bool DownloadFile(string fromUrl, string toFile)
    {
        HttpClient client = new HttpClient();

        try
        {
            byte[] bytes = client.GetByteArrayAsync(fromUrl).Result;

            using (FileStream fs = File.OpenWrite(toFile))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    internal static async Task<bool> DownloadFileAsync(string fromUrl, string toFile)
    {
        HttpClient client = new HttpClient();

        try
        {
            var downloadTask = client.GetByteArrayAsync(fromUrl);

            using (FileStream fs = File.OpenWrite(toFile))
            {
                byte[] bytes = await downloadTask;
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }
}