using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup( typeof( SignalR_Server.Startup ) )]
namespace SignalR_Server {
  public class Startup {
    public void Configuration( IAppBuilder app ) {
      Console.WriteLine( "Startup-Configuration" );
      app.Map( "/signalr", map =>
      {
        map.UseCors( CorsOptions.AllowAll );
        var hubConfiguration = new HubConfiguration
        {
          EnableDetailedErrors = true
        };
        map.RunSignalR( hubConfiguration );
      } );
    }
  }
}