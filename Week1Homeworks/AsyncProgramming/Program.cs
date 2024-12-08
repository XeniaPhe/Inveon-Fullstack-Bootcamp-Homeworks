using Xenia.InveonBootcamp.Homeworks.Week1.AsyncProgramming.AsyncAwaitUsage;

//It takes around 92 MBs of PI until 13 accurate decimal digits
const int NrApproximations = 92 * (1024 * 1024 / 16); //16 bytes per PI approximation in ASCII, 92 MBs of PI
const string piFile = "pi.txt";

Console.WriteLine("Synchronous: 0, Asynchronous: 1");
int choice = Convert.ToInt32(Console.ReadLine());

DeleteFile();

if (choice == 0)
{
    using (var api = PIApproximationService.CreateAndLaunch())
    {
        string url = api.GetUrlFor(NrApproximations);
        FileDownloader.DownloadFile(url, piFile);
    }
}
else
{
    await using (var api = PIApproximationService.CreateAndLaunch())
    {
        string url = api.GetUrlFor(NrApproximations);
        await FileDownloader.DownloadFileAsync(url, piFile);
    }
}

Console.WriteLine("File Ready");
Console.WriteLine("Press 1 to delete file");
choice = Convert.ToInt32(Console.ReadLine());

if (choice == 1)
{
    DeleteFile();
}

void DeleteFile()
{
    if (File.Exists(piFile))
    {
        File.Delete(piFile);
    }
}