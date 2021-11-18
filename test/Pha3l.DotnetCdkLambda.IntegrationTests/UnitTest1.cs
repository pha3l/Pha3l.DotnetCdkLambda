using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Pha3l.DotnetCdkLambda.IntegrationTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var url = Environment.GetEnvironmentVariable("ENDPOINT");

            using var http = new HttpClient();
            var resp = await http.GetAsync(url);
            var content = await resp.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK,resp.StatusCode);
            Assert.Equal("Hello from a lambda function!", content);
        }
    }
}