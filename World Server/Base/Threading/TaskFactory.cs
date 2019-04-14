using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorldServer.Base
{
    public class TaskExtensions<T>
    {
        public List<T> clients = new List<T>();
        public void AddToAction(T pram)
        {
            lock (clients)
            if (!clients.Contains(pram))
                clients.Add(pram);
        }
        CancellationTokenSource ts;
        public Task MyTask; 
        public TaskExtensions(Action<T> action, int Intreval, TaskCreationOptions TaskCreationOptions, TaskScheduler TaskScheduler)
        {
            //3291
            Console.WriteLine(action.Method.Name + " Task Start");
            ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            MyTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (ct.IsCancellationRequested)
                        {
                            Console.WriteLine(action.Method.Name + " Task Stop");
                            break;
                        }
                        if (TasksAction.StopActionWorld)
                        {
                            break;
                        }
                        await Task.Delay(Intreval, ct);
                        if (TasksAction.ShowMeDetailsWorld)
                        {
                            Stopwatch stopwatch = Stopwatch.StartNew();
                            foreach (var item in clients.ToArray())
                            {
                                action(item);
                            }
                            stopwatch.Stop();
                            Console.WriteLine("ID : " + Thread.CurrentThread.ManagedThreadId + "Action Name: [" + action.Method.Name + "]  Take : [" + stopwatch.Elapsed.TotalMilliseconds + "]");
                        }
                        else
                            foreach (var item in clients.ToArray())
                            {
                                action(item);
                            }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }, ct, TaskCreationOptions, TaskScheduler);
        }
        public void RemoveFromAction(T pram)
        {
            lock (clients)
            if (clients.Contains(pram))
                clients.Remove(pram);
        }
        public override string ToString()
        {
            return clients.Count.ToString();
        }
        public void Dispose()
        {
            if (ts != null)
                ts.Cancel();
        }
    }

    public class TaskExtensions
    {
        public static bool ShowMeDetails;
        public Task MyTask;
        public CancellationTokenSource ts;
        public static bool StopAction;
        CancellationToken ct;
        public TaskExtensions(Action action, int Intreval, TaskCreationOptions TaskCreationOptions, TaskScheduler TaskScheduler)
        {
            Console.WriteLine(action.Method.Name + " Task Start() ", ConsoleColor.Yellow, ConsoleColor.Green);
            ts = new CancellationTokenSource();
            ct = ts.Token;
            MyTask = new Task(() => mytask(action, Intreval), ct, TaskCreationOptions);
            MyTask.Start();
        }
        public async void mytask(Action action, int Intreval)
        {
            while (true)
            {
                try
                {
                    if (ts.IsCancellationRequested)
                    {
                        Console.WriteLine(action.Method.Name + " Task Stop");
                        break;
                    }
                    if (StopAction)
                    {
                        break;
                    }
                    await Task.Delay(Intreval, ct);
                    if (ShowMeDetails)
                    {
                        Stopwatch stopwatch = Stopwatch.StartNew();
                        action();
                        stopwatch.Stop();
                        Console.WriteLine("ID : " + Thread.CurrentThread.ManagedThreadId + "Action Name: [" + action.Method.Name + "]  Take : [" + stopwatch.Elapsed.TotalMilliseconds + "]");
                    }
                    else
                        action();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        public void Dispose()
        {
            try
            {
                ts.Cancel();  //cancel...
                MyTask.Wait();  //...and wait for the action within the task to complete
                MyTask.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
