namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.DIP.Good;
internal class CloudLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}