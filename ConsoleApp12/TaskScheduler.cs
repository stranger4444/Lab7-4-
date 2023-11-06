using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp12
{
    internal class TaskScheduler<TTask, TPriority>
    {
        private readonly List<TaskWithPriority> taskQueue = new List<TaskWithPriority>();
        private readonly Func<TTask, TPriority> getTaskPriority;
        private readonly Action<TTask> initializeTask;
        private readonly Action<TTask> resetTask;

        public delegate void TaskExecution<TTask>(TTask task);

        public TaskScheduler(
            Func<TTask, TPriority> getTaskPriority,
            Action<TTask> initializeTask,
            Action<TTask> resetTask)
        {
            this.getTaskPriority = getTaskPriority;
            this.initializeTask = initializeTask;
            this.resetTask = resetTask;
        }

        public void EnqueueTask(TTask task)
        {
            taskQueue.Add(new TaskWithPriority(task, getTaskPriority(task)));
        }

        public void ExecuteNext(TaskExecution<TTask> executeTask)
        {
            if (taskQueue.Count > 0)
            {
                TaskWithPriority taskToExecute = taskQueue
                    .OrderByDescending(t => t.Priority)
                    .First();
                TTask task = taskToExecute.Task;
                taskQueue.Remove(taskToExecute);

                initializeTask(task);
                executeTask(task);
                resetTask(task);
            }
        }

        public int GetTaskCount()
        {
            return taskQueue.Count;
        }

        public TTask GetTaskFromPool()
        {
            if (taskQueue.Count > 0)
            {
                TTask task = taskQueue[0].Task;
                taskQueue.RemoveAt(0);
                return task;
            }
            return default(TTask);
        }

        public void ReturnTaskToPool(TTask task)
        {
            taskQueue.Add(new TaskWithPriority(task, getTaskPriority(task)));
        }

        private class TaskWithPriority
        {
            public TTask Task { get; }
            public TPriority Priority { get; }

            public TaskWithPriority(TTask task, TPriority priority)
            {
                Task = task;
                Priority = priority;
            }
        }
    }
}
