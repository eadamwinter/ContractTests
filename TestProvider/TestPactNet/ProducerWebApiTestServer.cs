using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Provider2;
using System;

namespace TestPactNet;

// This WebApiProducer need Startup class from TestProvider class.

public class ProducerWebApiTestServer : IDisposable
{
    private readonly IHost server;
    public string ServerUrl { get { return "http://localhost:9050"; } }

    public ProducerWebApiTestServer()
    {

        server = Host.CreateDefaultBuilder()
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseUrls(ServerUrl);
                            webBuilder.UseStartup<Startup>();
                        })
                        .Build();

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
