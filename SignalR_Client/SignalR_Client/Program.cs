using System;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;

namespace SignalR_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting client  http://localhost:8089");            
            var hubConnection = new HubConnection("http://localhost:8089/");
            //hubConnection.TraceLevel = TraceLevels.All;
            //hubConnection.TraceWriter = Console.Out;
            IHubProxy myHubProxy = hubConnection.CreateHubProxy("MyHub");
            

            myHubProxy.On<string, string>("BroadCast_Message", (name, message) => DoSomethingWithTheMessage(name, message));
            myHubProxy.On("heartbeat", () => DoSomethingonHeartbeat()); 
            hubConnection.Start().Wait();

            while (true)
            {
                string key = Console.ReadLine();
                if (key.ToUpper() == "W")
                {
                    Console.WriteLine("I am now sending a message to the server, which will be then redistributed to all clients ...... ");
                    myHubProxy.Invoke("BroadCast_Message", Environment.UserName, " sent from console client at " + DateTime.Now.ToLongTimeString()).ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine("!!! There was an error opening the connection:{0} \n", task.Exception.GetBaseException());
                        }

                    }).Wait();                    
                }

                if (key.ToUpper() == "H")
                {
                    Console.WriteLine("I am now sending a heartbeat to the server ...... ");
                    myHubProxy.Invoke("Heartbeat").ContinueWith(task =>
                    {
                        if (task.IsFaulted)
                        {
                            Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                        }

                    }).Wait();
                   
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
            Console.WriteLine("Message from >>> {0}  with content >>> {1}   ", name, message);
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
