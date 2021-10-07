using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AppMetadataAPIServer.Models;
using AppMetadataAPIServer.Query;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AppMetadataAPIServer.IntegrationTest
{
    [TestClass]
    public class IntegrationTest
    {
        private HttpClient client;

        private const string Endpoint = "v1/metadata";
        
        private IDeserializer yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(LowerCaseNamingConvention.Instance)
            .Build();

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

        [TestMethod]
        public async Task ShouldQueryCorrectly()
        {
            string[] validPayloadFiles = Directory.GetFiles("test-data/valid-inputs").OrderBy(_=>_).ToArray();
            IList<ApplicationMetadata> appMetadata = new List<ApplicationMetadata>();
            foreach (string payloadFile in validPayloadFiles)
            {
                string payload = File.ReadAllText(payloadFile);
                appMetadata.Add(this.yamlDeserializer.Deserialize<ApplicationMetadata>(payload));
                await client.PostAsync(Endpoint, new StringContent(payload));
            }
            HttpResponseMessage response = await client.GetAsync($"{Endpoint}?title=Valid App 1&version=0.0.1");
            var queryResult =await GetQueryResultFromResponse(response);
            queryResult.IsSuccess.Should().BeTrue();
            queryResult.resultData.Should().BeEquivalentTo(new List<ApplicationMetadata>(){appMetadata[0]});
            
            response = await client.GetAsync($"{Endpoint}?title=Valid App 2");
            queryResult = await GetQueryResultFromResponse(response);
            queryResult.IsSuccess.Should().BeTrue();
            queryResult.resultData.Should().BeEquivalentTo(new List<ApplicationMetadata>(){appMetadata[2], appMetadata[3]});
            
            response = await client.GetAsync($"{Endpoint}?name=secondmaintainer app1&website=https://website.com");
            queryResult = await GetQueryResultFromResponse(response);
            queryResult.IsSuccess.Should().BeTrue();
            queryResult.resultData.Should().BeEquivalentTo(
                new List<ApplicationMetadata>(){appMetadata[0], appMetadata[1], appMetadata[2]});
            
            response = await client.GetAsync($"{Endpoint}?name=kai&website=https://website.com&license=apache");
            queryResult = await GetQueryResultFromResponse(response);
            queryResult.IsSuccess.Should().BeTrue();
            queryResult.resultData.Should().BeEmpty();
        }

        private async Task<QueryResult> GetQueryResultFromResponse(HttpResponseMessage response)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            QueryResult result = JsonSerializer.Deserialize<QueryResult>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result;
        }
    }
}