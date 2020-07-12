using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Xunit;
using System.Net.Http;
using StoriesWebApi;
using System.Threading.Tasks;
using System.Net;

namespace StoriesIntegrationTest
{
    public class StoriesIntegrationTest
    {
        private readonly HttpClient httpClient;
        public StoriesIntegrationTest()
        {
            var server = new TestServer(new WebHostBuilder().UseEnvironment("Development").UseStartup<Startup>());
            httpClient = server.CreateClient();         
        }

        [Theory]
        [InlineData("GET")]
        public async Task TestApiStories(string method)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/stories");

            var response = await httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
