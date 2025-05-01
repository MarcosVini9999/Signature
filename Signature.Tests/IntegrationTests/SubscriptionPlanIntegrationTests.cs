using System.Net.Http.Json;
using System.Net;
using Signature.API.Application.DTOs;

namespace Signature.Tests.IntegrationTests
{
    public class SubscriptionPlanIntegrationTests :
        IClassFixture<SignatureApiFactory>
    {
        private readonly HttpClient _client;

        public SubscriptionPlanIntegrationTests(SignatureApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_Plans_DeveRetornarPeloMenosUm()
        {
            var response = await _client.GetAsync("/api/subscriptionplans");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var list = await response.Content.ReadFromJsonAsync<List<SubscriptionPlanDto>>();
            Assert.NotNull(list);
            Assert.NotEmpty(list);
        }

        [Theory]
        [InlineData("Bronze", 20.0)]
        [InlineData("Prata", 50.5)]
        [InlineData("Ouro", 99.9)]
        public async Task Post_Plan_Valido_DeveCriarNovoPlano(string title, decimal price)
        {
            var dto = new CreateSubscriptionPlanDto(title, price);

            var response = await _client.PostAsJsonAsync("/api/subscriptionplans", dto);

            response.EnsureSuccessStatusCode();
            var created = await response.Content.ReadFromJsonAsync<SubscriptionPlanDto>();

            Assert.NotNull(created);
            Assert.Equal(title, created.Title);
            Assert.Equal(price, created.Price);
        }

        [Fact]
        public async Task GetAll_Order_ShouldReturnInsertionOrder()
        {
            await _client.PostAsJsonAsync("/api/subscriptionplans", new CreateSubscriptionPlanDto("A", 1));
            await _client.PostAsJsonAsync("/api/subscriptionplans", new CreateSubscriptionPlanDto("B", 2));

            var response = await _client.GetAsync("/api/subscriptionplans");
            var list = await response.Content.ReadFromJsonAsync<List<SubscriptionPlanDto>>();

            Assert.NotNull(list);
            var titles = list.Select(p => p.Title).ToList();
            Assert.Contains("A", titles);
            Assert.Contains("B", titles);

            var indexA = titles.IndexOf("A");
            var indexB = titles.IndexOf("B");
            Assert.True(indexA < indexB,
                $"O plano A deveria vir antes de B, mas A está em {indexA} e B em {indexB}");
        }
    }
}
