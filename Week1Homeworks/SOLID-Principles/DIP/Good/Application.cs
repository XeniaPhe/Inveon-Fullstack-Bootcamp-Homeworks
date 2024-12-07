namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.DIP.Good;
internal class Application(ILogger logger)
{
    internal void Run()
    {
        logger.Log("Logged");
    }
}