using System;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SignalR_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Underlying instance = new Underlying();
            Thread bThread = new Thread(new ThreadStart(instance.Manager));
            bThread.Start();
            //bThread.Join();  // wait or not
            
            Console.WriteLine("Continuing");
            Thread.Sleep(20000);

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }




   class Underlying
   {
        public void Manager()
        {
            Startup().Wait(); // blocking. I cannot do async all the way up here as console app do not support it.   
        }
        
        private async static Task Startup()
        {
            Console.WriteLine("Starting client  http://localhost:8089");
            var hubConnection = new HubConnection("http://localhost:8089/");
            //hubConnection.TraceLevel = TraceLevels.All;
            //hubConnection.TraceWriter = Console.Out;
            IHubProxy myHubProxy = hubConnection.CreateHubProxy("MyHub");
            myHubProxy.On<string, string>("BroadCast_Message", (name, message) => DoSomethingWithTheMessage(name, message));
            myHubProxy.On("heartbeat", () => DoSomethingonHeartbeat());
            await hubConnection.Start();

            while (true)
            {
                string key = Console.ReadLine();

                if (key.ToUpper() == "B") // Broadcast
                {
                    Console.WriteLine("I am now sending a message to the server, which will be then redistributed to all clients ...... ");
                    await myHubProxy.Invoke("BroadCast_Message", Environment.UserName, " sent from console client at (ticks) " + DateTime.Now.Ticks.ToString()).ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine("!!! There was an error opening the connection:{0} \n", task.Exception.GetBaseException());
                        }

                    });
                }

                if (key.ToUpper() == "H") // Heartbeat
                {
                    Console.WriteLine("I am now sending a heartbeat to the server ...... ");
                    await myHubProxy.Invoke("Heartbeat").ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                        }

                    }); ;

                }

                if (key.ToUpper() == "C")
                {
                    break;
                }
            }
            
        }


        


        private static void DoSomethingWithTheMessage(string name, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            long diff = DateTime.Now.Ticks - Convert.ToInt64(Regex.Match(message, @"\d+").Value); 
            Console.WriteLine("Message from >>> {0}  with content >>> {1} transport in ticks  took >>> {2} ", name, message, diff.ToString());
            Console.ResetColor();
        }

        private static void DoSomethingonHeartbeat()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Recieved heartbeat \n");
            Console.ResetColor();
        }






    }
}
