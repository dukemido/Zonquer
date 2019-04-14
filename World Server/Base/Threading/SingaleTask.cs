using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorldServer.Base
{
    public static class TasksAction
    {
        public static bool ShowMeDetails;
        public static bool ShowMeDetailsWorld;
        public static bool StopAction;
        public static bool StopActionSp;
        public static bool StopActionWorld;
        public static int count;
    }
    public class SingaleTask<T>
    {
        public T holder;
        public Task MyTask;
        CancellationTokenSource ts;
        public bool Breakwhile;

        public SingaleTask(Action<T> action, int Intreval, TaskCreationOptions TaskCreationOptions, TaskScheduler TaskScheduler, T parm, List<SingaleTask<T>> List)
        {
           // Console.WriteLine(action.Method.Name + "Task Start() ", ConsoleColor.Yellow, ConsoleColor.Green);
            ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            List.Add(this);
            holder = parm;
            MyTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (ct.IsCancellationRequested || Breakwhile)
                        {
                            //  Console.WriteLine(action.Method.Name + "Task Stop");
                            break;
                        }
                        if (TasksAction.StopAction)
                        {
                            await Task.Delay(5000, ct);
                            Console.WriteLine(action.Method.Name + "Task puse");
                            continue;
                        }
                        await Task.Delay(Intreval, ct);
                        if (holder != null)
                        {
                            if (TasksAction.ShowMeDetails)
                            {
                                Stopwatch stopwatch = Stopwatch.StartNew();
                                action(holder);
                                stopwatch.Stop();
                                Console.WriteLine("ID : "+Thread.CurrentThread.ManagedThreadId+"Action Name: [" + action.Method.Name + "]  Take : [" + stopwatch.Elapsed.TotalMilliseconds + "]");
                            }
                            else
                                action(holder);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }, ct, TaskCreationOptions, TaskScheduler);
        }
        public void Dispose()
        {
            try
            {
                //if (ts != null)
                //    ts.Cancel();
                MyTask.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
    public class SpicalSingaleTask<T>
    {
        public T holder;
        public Task MyTask;
        public bool Breakwhile;
        CancellationTokenSource ts;
        public SpicalSingaleTask(Action<T> action, int Intreval, TaskCreationOptions TaskCreationOptions, TaskScheduler TaskScheduler, T parm, List<SpicalSingaleTask<T>> List)
        {
            TasksAction.count++;
            // Console.WriteLine(action.Method.Name + "Task Start() ", ConsoleColor.Yellow, ConsoleColor.Green);
            ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            List.Add(this);
            holder = parm;
            MyTask = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (ct.IsCancellationRequested || Breakwhile)
                        {
                            //  Console.WriteLine(action.Method.Name + "Task Stop");
                            break;
                        }
                        if (TasksAction.StopActionSp)
                        {
                            await Task.Delay(5000, ct);
                            Console.WriteLine(action.Method.Name + "Task puse");
                            continue;
                        }
                        await Task.Delay(Intreval, ct);
                        if (holder != null)
                        {
                            if (TasksAction.ShowMeDetails)
                            {
                                Stopwatch stopwatch = Stopwatch.StartNew();
                                action(holder);
                                stopwatch.Stop();
                                Console.WriteLine("ID : " + Thread.CurrentThread.ManagedThreadId + "Action Name: [" + action.Method.Name + "]  Take : [" + stopwatch.Elapsed.TotalMilliseconds + "]");
                            }
                            else
                                action(holder);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }, ct, TaskCreationOptions, TaskScheduler);
        }
        public void Dispose()
        {
            try
            {
                //if (ts != null)
                //    ts.Cancel();
                MyTask.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

}
