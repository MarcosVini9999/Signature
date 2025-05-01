using Signature.API.Application.DTOs;
using System.Net.Http.Json;
using System.Net;

namespace Signature.Tests.IntegrationTests
{
    [TestCaseOrderer(
        ordererTypeName: "XUnit.Project.Orderers.AlphabeticalOrderer",
        ordererAssemblyName: "Signature.Tests.IntegrationTests")]
    public class SistemaTests : IClassFixture<SistemaFixture>
    {
        private readonly HttpClient _client;
        private readonly Guid _userId;
        private static Guid _planId;

        public SistemaTests(SistemaFixture fixture)
        {
            _client = fixture.Client;
            _userId = fixture.CreatedUserId;
        }

        [Fact]
        public async Task Test1_CriarPlano_DeveRetornarCreated()
        {
            var dto = new CreateSubscriptionPlanDto("SysPlan", 49.9m);

            var resp = await _client.PostAsJsonAsync(
                "/api/subscriptionplans", dto);

            Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
            var created = await resp.Content.ReadFromJsonAsync<SubscriptionPlanDto>()!;
            Assert.Equal("SysPlan", created.Title);
            Assert.Equal(49.9m, created.Price);

            _planId = created.Id;
        }

        [Fact]
        public async Task Test2_AssinarPlano_DeveRetornarOkComMensagem()
        {
            var url = $"/api/subscriptions/subscribe?userId={_userId}&planId={_planId}";
            var resp = await _client.PostAsync(url, (HttpContent?)null);

            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
            var body = await resp.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            Assert.NotNull(body);
            Assert.True(body!.ContainsKey("Message"));
            Assert.Equal("Subscription successful.", body["Message"]);
        }

        [Fact]
        public async Task Test3_GetUsuario_DeveRetornarMesmoUsuario()
        {
            var resp = await _client.GetAsync($"/api/users/{_userId}");

            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
            var user = await resp.Content.ReadFromJsonAsync<UserDto>()!;
            Assert.Equal(_userId, user.Id);
            Assert.Equal("Marco", user.Name);
            Assert.Equal("marco@ex.com", user.Email);
        }

        [Fact]
        public async Task Test4_GetPlans_DeveConterSysPlan()
        {
            var resp = await _client.GetAsync("/api/subscriptionplans");

            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
            var list = await resp.Content.ReadFromJsonAsync<List<SubscriptionPlanDto>>()!;
            Assert.NotNull(list);
            Assert.Contains(list, p => p.Id == _planId && p.Title == "SysPlan");
        }
    }
}
