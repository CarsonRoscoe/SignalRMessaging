using System;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

public class ServerProgram {
  bool m_continue = false;
  public ServerProgram() {
    StartThread();
  }

  public void StartThread() {
    m_continue = true;
    var task = Task.Run( () =>
    {
      var url = "http://localhost:8089/";
      try {
        using ( WebApp.Start( url ) ) {
          Console.WriteLine( "Server Program on {0}", url );
          while ( m_continue ) {

          }
        }
      }
      catch ( Exception e ) {
        Console.WriteLine( "ServerProgram->StartThread Failed. If error is about HTTP access, run command:\nnetsh http add urlacl url=http://localhost:8080/ user=Everyone listen=yes" );
        Console.WriteLine( e.Message );
      }
    } );
  }

  public void EndThread() {
    m_continue = false;
  }
}
