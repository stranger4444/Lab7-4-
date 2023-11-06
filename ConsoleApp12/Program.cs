using ConsoleApp12;
class Program
{
    static void Main()
    {
        TaskScheduler<string, int> scheduler = new TaskScheduler<string, int>(
                getTaskPriority: task => task.Length,
                initializeTask: task => Console.WriteLine($"Initializing task: {task}"),
                resetTask: task => Console.WriteLine($"Resetting task: {task}")
            );

        string input;
        do
        {
            Console.WriteLine("Enter a task (or press Enter to execute): ");
            input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                scheduler.EnqueueTask(input);
            }
            else if (scheduler.GetTaskCount() > 0)
            {
                TaskScheduler<string, int>.TaskExecution<string> executeTask = task =>
                {
                    Console.WriteLine($"Executing task: {task}");

                };
                scheduler.ExecuteNext(executeTask);
            }
        } while (!string.IsNullOrEmpty(input));
    }
} 
