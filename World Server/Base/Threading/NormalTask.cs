using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorldServer.Base
{
    public class NormalTask
    {
        public Task MyTask;
        public static bool ShowMeDetails;
        public static bool StopAction;
        CancellationTokenSource ts;
        public NormalTask(Action action, int Intreval)
        {
            Console.WriteLine(action.Method.Name + " Task Start() ", ConsoleColor.Yellow, ConsoleColor.Green);
            ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            MyTask = Task.Run(async () =>
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
            }, ct);
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
