using Microsoft.VisualBasic;

namespace Xenia.InveonBootcamp.Homeworks.Week1.AsyncProgramming.AsyncAwaitUsage;
internal static class TaskClassStaticMethods
{
    internal static void Run()
    {
        //Used for running a task on a worker thread
        Task.Run(() =>
        {
            for (int i = 0; i < 100000; i++)
            {
                Console.WriteLine(i);
            }
        });
    }

    internal static void WaitAll()
    {
        var taskA = Task.Run(() => { });
        var taskB = Task.Run(() => { });
        var taskC = Task.Run(() => { });
        var taskD = Task.Run(() => { });
        var taskE = Task.Run(() => { });

        //A synchronous (blocking) method that waits for all the given tasks to complete (can be cancelled too)
        //Shouldn't be used in async methods since it defeats the whole purpose
        Task.WaitAll([taskA, taskB, taskC, taskD, taskE]);
    }

    internal static async Task WhenAll()
    {
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => { }));
        }

        //An asynchronous (nonblocking) method that returns a task that completes when all given tasks are completed
        await Task.WhenAll(tasks);
    }

    internal static void WaitAny()
    {
        Task[] tasks = new Task[10];

        for (int i = 0; i < 10; i++)
        {
            tasks[i] = Task.Run(() => { });
        }

        int completed = 0;

        //A sychronous (blocking) method call that returns when any of the tasks given to it is completed
        //Can be used to divide a work into multiple pieces and process each piece as they complete
        //Should be avoided in async methods since it is a blocking call

        while (completed != tasks.Length)
        {
            int completedTaskIndex = Task.WaitAny(tasks);
            completed++;
            //Process data
        }
    }

    internal static async Task WhenAny()
    {
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => { }));
        }

        //An asychronous (nonblocking) method call that returns a task that completes when any of the tasks given to it is completed
        //Can be used to divide a work into multiple pieces and process each piece as they complete

        int completed = 0;
        while (completed != tasks.Count)
        {
            var completedTask = await Task.WhenAny(tasks);
            completed++;
            //Process data
        }
    }

    internal static void Delay()
    {
        //Used for creating a task that completes after a specified amount of time
        Task.Delay(TimeSpan.FromSeconds(30));

        //Could be useful for implementing timeout for other tasks or implementing cancellation for normally uncancellable tasks:
        var mainTask = Task.Run(() =>
        {
            Console.WriteLine("Main task");
        });
        
        CancellationTokenSource cts = new CancellationTokenSource();
        var delayTask = Task.Delay(TimeSpan.FromSeconds(30), cts.Token);

        Task.WhenAny(mainTask, delayTask).ContinueWith(task =>
        {
            if (task == mainTask)
                return;

            if (cts.IsCancellationRequested)
            {
                Console.WriteLine("Task cancelled!");
            }
            else
            {
                Console.WriteLine("Task timed out!");
            }
        });
    }

    internal static void FromException(string arg, int arg1)
    {
        //Creates a task that has completed with an exception
        //Can be used to return from non-async methods that return Task objetcs
        Task.FromException(new ArgumentNullException(nameof(arg)));

        //Supports generic tasks too:
        Task<int> erronousTask = Task.FromException<int>(new ArgumentOutOfRangeException(nameof(arg1), arg1, "Out of range"));
    }

    internal static void FromCancelled()
    {
        //Creates a pre-cancelled task
        //Could be an alternative to throwing exceptions, especially if you don't want the extra overhead
        CancellationTokenSource cts = new CancellationTokenSource();
        var cancelledTask = Task.FromCanceled(cts.Token);

        //Supports generic Tasks too:
        Task<int> cancelledTask1 = Task.FromCanceled<int>(cts.Token);
    }

    internal static void FromResult()
    {
        //Creates a successfully completed task with the value given:
        //Can be used for returning values from non-async methods that return Task objects
        Task<int> task = Task.FromResult<int>(1000);
    }

    internal static async Task WhenEach()
    {
        //Can be used when you have multiple tasks running and want to process their results as they return
        //It basically functions like an async coroutine and it's much simpler than using WhenAny

        List<Task> tasks = new List<Task>();
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Run(() => { }));
        }

        await foreach (var task in Task.WhenEach(tasks))
        {
            //Process task
        }
    }

    internal static async Task Yield()
    {
        //Can be used to yield control to other tasks or threads, like a coroutine
        //The current method will resume execution later
        //Useful for dividing long tasks into chunks
        //Or even forcing asynchronous behavior onto otherwise synchronous methods, for example:
        //Yielding to keep the UI responsive during a long lasting calculation
        await Task.Yield();
    }
}