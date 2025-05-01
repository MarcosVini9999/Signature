using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Signature.API.Application.DTOs;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Signature.Tests.StepDefinitions
{
    [Binding]
    public class UserSteps : IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private HttpResponseMessage _response;

        public UserSteps()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.UseEnvironment("Integration"));
            _client = _factory.CreateClient();
        }

        [Given("the API is running")]
        public void GivenApiIsRunning() { }

        [When("I send a POST request to \"(.*)\" with:")]
        public async Task WhenISendPost(string url, Table table)
        {
            var dto = table.CreateInstance<CreateUserDto>();
            _response = await _client.PostAsJsonAsync(url, dto);
        }

        [Then("the response status should be Created")]
        public void ThenStatusCreated()
        {
            _response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Then("the Location header should contain \"(.*)\"")]
        public void ThenLocationHeaderContains(string partialPath)
        {
            var locationUri = _response.Headers.Location;
            locationUri.Should().NotBeNull();
            locationUri!.AbsolutePath.ToLowerInvariant().Should().Contain(partialPath.ToLowerInvariant());
        }

        [Then("the response body should be a UserDto with:")]
        public async Task ThenResponseBody(Table table)
        {
            _response.Content.Headers.ContentLength.Should().BeGreaterThan(0);
            var expected = table.CreateInstance<UserDto>();
            var actual = await _response.Content.ReadFromJsonAsync<UserDto>();

            actual.Should().NotBeNull();
            actual!.Id.Should().NotBe(Guid.Empty);
            actual.Name.Should().Be(expected.Name);
            actual.Email.Should().Be(expected.Email);
        }

        public void Dispose()
        {
            _client?.Dispose();
            _response?.Dispose();
            _factory?.Dispose();
        }
    }
}
