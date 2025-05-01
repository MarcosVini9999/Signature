using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using TechTalk.SpecFlow;


namespace Signature.Tests.Steps
{
    [Binding]
    public class ApiSteps
    {
        private static WebApplicationFactory<Program> factory;
        private static HttpClient client;
        private HttpResponseMessage response;
        private readonly ScenarioContext context;

        public ApiSteps(ScenarioContext scenarioContext)
        {
            context = scenarioContext;
            if (factory == null)
            {
                factory = new WebApplicationFactory<Program>()
                    .WithWebHostBuilder(b => b.UseEnvironment("Integration"));
                client = factory.CreateClient();
            }
        }

        [When("I POST to \"(.*)\" with body:")]
        public async Task WhenIPost(string url, string body)
        {
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            response = await client.PostAsync(url, content);
        }

        [When("I POST to \"(.*)\" with no body")]
        public async Task WhenIPostNoBody(string url)
        {
            response = await client.PostAsync(url, null);
        }

        [When("I GET to \"(.*)\"")]
        public async Task WhenIGet(string url)
        {
            url = ReplacePlaceholders(url);
            response = await client.GetAsync(url);
        }

        [Then("the response status code should be (.*)")]
        public void ThenStatusCode(int expected)
        {
            ((int)response.StatusCode).Should().Be(expected);
        }

        [Then("the response JSON field \"(.*)\" should be \"(.*)\"")]
        public async Task ThenFieldEquals(string field, string expected)
        {
            var json = await response.Content.ReadFromJsonAsync<Newtonsoft.Json.Linq.JObject>();
            json[field].ToString().Should().Be(expected);
        }

        [Then("I save the \"(.*)\" from the response as \"(.*)\"")]
        public async Task ThenSaveField(string field, string key)
        {
            var json = await response.Content.ReadFromJsonAsync<Newtonsoft.Json.Linq.JObject>();
            context[key] = Guid.Parse(json[field].ToString());
        }

        private string ReplacePlaceholders(string url)
        {
            foreach (var kv in context)
                url = url.Replace($"{{{kv.Key}}}", kv.Value.ToString());
            return url;
        }
    }
}
