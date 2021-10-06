using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppMetadataAPIServer.IntegrationTest
{
    [TestClass]
    public class IntegrationTest
    {
        private HttpClient client;

        private const string Endpoint = "v1/metadata";

        [TestInitialize]
        public void TestInitialize()
        {
            WebApplicationFactory<Startup> factory = new WebApplicationFactory<Startup>();
            this.client = factory.CreateDefaultClient();
        }
        
        [TestMethod]
        public async Task ShouldReturnSuccessEmptyWhenNoQueryParameters()
        {
            HttpResponseMessage response = await client.GetAsync(Endpoint);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            string responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().BeEquivalentTo("{\"isSuccess\":true,\"resultData\":[]}");
        }

        [TestMethod]
        public async Task ShouldCreateForValidPayloads()
        {
            string[] validPayloadFiles = Directory.GetFiles("test-data/valid-inputs");
            foreach (string payloadFile in validPayloadFiles)
            {
                Console.WriteLine($"testing file: {payloadFile}");
                string payload = File.ReadAllText(payloadFile);
                var response = await client.PostAsync(Endpoint, new StringContent(payload));
                response.StatusCode.Should().Be(HttpStatusCode.Accepted);
                string responseBody = await response.Content.ReadAsStringAsync();
                responseBody.Should().BeEquivalentTo("OK");
            }
        }

        [TestMethod]
        public async Task ShouldRejectInvalidPayloads()
        {
            string[] invalidPayloadFiles = Directory.GetFiles("test-data/invalid-inputs"); 
            foreach (string payloadFile in invalidPayloadFiles)
            {
                Console.WriteLine($"testing file: {payloadFile}");
                string payload = File.ReadAllText(payloadFile);
                var response = await client.PostAsync(Endpoint, new StringContent(payload));
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                string responseBody = await response.Content.ReadAsStringAsync();
                responseBody.Should().NotBeEmpty();
            }
        }

        [TestMethod]
        public async Task ShouldRejectDuplicateRecords()
        {
            string payloadPath = "test-data/valid-inputs/validInput1.yaml";
            string payload = File.ReadAllText(payloadPath); 
            var response = await client.PostAsync(Endpoint, new StringContent(payload));
            response.StatusCode.Should().Be(HttpStatusCode.Accepted);
            
            response = await client.PostAsync(Endpoint, new StringContent(payload));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
 
            string responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Be("Metadata already exists.");
        }
    }
}