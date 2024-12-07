namespace Xenia.InveonBootcamp.Homeworks.Week1.SolidPrinciples.DIP.Bad;

/*
 * Why does this code violate the Dependency Inversion Principle (DIP)?
 * 
 * The Application class directly depends on the concrete FileLogger class. This creates a tight coupling 
 * between the Application class and the FileLogger, making it difficult to change the logging behavior 
 * without modifying the Application class itself. 
 * 
 * According to DIP, high-level modules (like Application) should not depend on low-level modules (like FileLogger). 
 * Instead, both should depend on abstractions (such as an ILogger interface). This allows for easier maintenance 
 * and flexibility, as different logging mechanisms can be introduced without modifying the Application class.
 */

internal class Application
{
    private FileLogger logger = new FileLogger();

    internal void Run()
    {
        logger.Log("Logged");
    }
}