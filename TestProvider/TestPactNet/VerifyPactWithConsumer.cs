using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Verifier;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace TestPactNet;

// Change Producer Type to ProducerWebApiTestServer or ProducerWebApiTestServer2
public class VerifyPactWithConsumer : IClassFixture<ProducerWebApiTestServer>
{
    private readonly ProducerWebApiTestServer _testServer;
    private readonly ITestOutputHelper _output;

    public VerifyPactWithConsumer(ProducerWebApiTestServer testServer, ITestOutputHelper output)
    {
        _testServer = testServer;
        _output = output;
    }

    [Fact]
    public void VerifyPact()
    {
        var config = new PactVerifierConfig
        {
            LogLevel = PactLogLevel.Information,
            Outputters = new List<IOutput>()
            {
                new XUnitOutput(_output)
            }
        };

        string pactPath = Path.Join("..",
                                    "..",
                                    "..",
                                    "pacts",
                                    "Consumer API-Provider API.json");

        IPactVerifier verifier = new PactVerifier(config);

        verifier
                .ServiceProvider("Profile API", new Uri(_testServer.ServerUrl))
                .WithFileSource(new FileInfo(pactPath))
                .WithRequestTimeout(TimeSpan.FromMinutes(20))
                .Verify();
    }
}
