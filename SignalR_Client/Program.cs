using System;
using System.Threading;

namespace SignalR_Client {
  class Program {
    static void Main( string[] args ) {
      var server = new ServerProgram();
      Thread.Sleep( 1000 );
      var client = new ClientProgram();
      Thread.Sleep( 1000 );
      //client.SendMessage( "Carson", "Test" ).Wait();
      //Thread.Sleep( 1000 );
      Console.WriteLine( "[Any key to quit]" );
      Console.ReadLine();
    }
  }
}
