using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace TestPactNet;

// This WebApiProducer doesn't need Startup class
public class ProducerWebApiTestServer2 : IDisposable
{
    private readonly IHost server;

    // it doesn't matter what port number you put here
    public string ServerUrl { get { return "http://localhost:9090"; } }

    public ProducerWebApiTestServer2()
    {
        Assembly referencedAssembly = typeof(Provider2.Program).Assembly;
        string assemblyName = referencedAssembly.GetName().Name;

        server = new HostBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
         {
             webBuilder.UseUrls(ServerUrl)
             .ConfigureServices(services =>
              {
                  services.AddControllers().AddApplicationPart(referencedAssembly);
              })
                    .Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    });

         }).Build();

        server.Start();
    }

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                server.StopAsync().GetAwaiter().GetResult();
                server.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose() => Dispose(true);
}
