using PactNet;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TestConsumer;
using Xunit;

namespace TestPactNet;

public class ConsumerPactTest
{
    private readonly IPactBuilderV3 pactBuilder;

    public ConsumerPactTest()
    {
        var pact = Pact.V3("Consumer API", "Provider API", new PactConfig());
        pactBuilder = pact.WithHttpInteractions();
    }

    [Fact]
    public async Task EnsureProducerHonorsContract()
    {
        pactBuilder
            .UponReceiving("A GET request to /api/data")
                .Given("There is available data")
                .WithRequest(HttpMethod.Get, "/api/data")
            .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(new
                {
                    data = new List<string> { "Value1", "Value2" }
                });

        await pactBuilder.VerifyAsync(async ctx =>
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = ctx.MockServerUri;
            var provider = new ProviderService(httpClient);
            var response = await provider.GetData();
            Assert.NotEmpty(response.Data);
        });
    }
}
