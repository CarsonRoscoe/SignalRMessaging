using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

[HubName( nameof( ServerHub ) )]
public class ServerHub : Hub {
  public void BroadCastMessage( string name, string message ) {
    Console.WriteLine( "[Server] Message From: {0}\nMessage: {1}", name, message );
    Clients.All.BroadCastMessage( name, message );
  }

  public override Task OnConnected() {
    Console.WriteLine( "[Server] Client has connected id: {0} ", Context.ConnectionId );
    return (base.OnConnected());
  }

  public override Task OnDisconnected( bool stopCalled ) {
    Console.WriteLine( "[Server] Client has disconnected id: {0} ", Context.ConnectionId );
    return (base.OnDisconnected( stopCalled ));
  }

  public override Task OnReconnected() {
    Console.WriteLine( "[Server] Client has reconnected id: {0} ", Context.ConnectionId );
    return (base.OnReconnected());
  }
}
