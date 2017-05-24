using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;

public class ClientProgram {
  HubConnection m_hubConnection;
  IHubProxy m_myHubProxy;
  public ClientProgram() {
    System.Net.ServicePointManager.DefaultConnectionLimit = 5;
    Task.Run( async() => await Startup() );
  }

  public async Task SendMessage( string name, string message ) {
    Console.WriteLine( "ClientProgram->SendMessage" );
    await m_myHubProxy.Invoke( nameof( ServerHub.BroadCastMessage ), name, message ).ContinueWith( task =>
    {
      if ( task.IsFaulted ) {
        Console.WriteLine( "!!! There was an error opening the connection:{0} \n", task.Exception.GetBaseException() );
      }
      else {
        Console.WriteLine( "ClientProgram->SendMessage:BroadCastMessage Success" );
      }
    } );
  }

  private async Task Startup() {
    Console.WriteLine( "ClientProgram on http://localhost:8089" );
    m_hubConnection = new HubConnection( "http://localhost:8089" );
    m_myHubProxy = m_hubConnection.CreateHubProxy( nameof( ServerHub ) );
    m_myHubProxy.On<string, string>( nameof( ServerHub.BroadCastMessage ), ( name, message ) => OnMessageReceived( name, message ) );
    m_hubConnection.Closed += OnClosed;
    m_hubConnection.Error += OnErrorReceived;
    m_hubConnection.StateChanged += M_hubConnection_StateChanged;
    await m_hubConnection.Start(new WebSocketTransport()); //Hangs
    await SendMessage( "Startup", "Sending from same thread as connection established" ); //Never hit
  }

  private void M_hubConnection_StateChanged( StateChange obj ) {
    Console.WriteLine( "State Changed to " + obj.NewState.ToString() );
  }

  private void OnErrorReceived( Exception obj ) {
    throw obj; //Never called, no errors
  }

  private void OnClosed() {
    Console.WriteLine( "Client Connection Closed" );
  }

  private static void OnMessageReceived( string name, string message ) {
    Console.WriteLine( "From {0}:\nMessage: {1} ", name, message );
  }
}
