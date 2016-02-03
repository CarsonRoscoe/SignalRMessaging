using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace SignalR_Server
{
    public class MyHub : Hub
    {
        public void BroadCast_Message(string name, string message)
        {
            Console.WriteLine("Message from >>> {0} with content >>> {1}", name, message);
            Clients.All.BroadCast_Message(name, message);
        }

        public void Heartbeat()
        {
            Console.WriteLine("Heartbeat ");
            Clients.All.heartbeat();
        }

        public override Task OnConnected()
        {
            Console.WriteLine("Client has connected id: {0} ", Context.ConnectionId);
            return (base.OnConnected());
        }
        
        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine("Client has disconnected id: {0} ", Context.ConnectionId);
            return (base.OnDisconnected(stopCalled));
        }

        public override Task OnReconnected()
        {
            Console.WriteLine("Client has reconnected id: {0} ", Context.ConnectionId);
            return (base.OnReconnected());
        }




    }
}