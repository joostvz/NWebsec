// Copyright (c) Andr� N. Klingsheim. See License.txt in the project root for license information.

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using NWebsec.AspNetCore.Mvc.FunctionalTests.Plumbing;

namespace NWebsec.AspNetCore.Mvc.FunctionalTests.Attributes
{
    [TestFixture]
    public class XXssProtectionTests
    {
        private TestServer _server;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _server = TestServerBuilder<MvcAttributeWebsite.Startup>.CreateTestServer();
            _httpClient = _server.CreateClient();
        }

        [TearDown]
        public void Cleanup()
        {
            _server.Dispose();
        }

        [Test]
        public async Task XXssProtection_Enabled_SetsHeaders()
        {
            const string path = "/XXssProtection";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.IsTrue(response.Headers.Contains("X-Xss-Protection"), path);
        }

        [Test]
        public async Task XXssProtection_Disabled_NoHeaders()
        {
            const string path = "/XXssProtection/Disabled";

            var response = await _httpClient.GetAsync(path);

            Assert.IsTrue(response.IsSuccessStatusCode, $"Request failed: {path}");
            Assert.IsFalse(response.Headers.Contains("X-Xss-Protection"), path);
        }
    }
}