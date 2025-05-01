using System.Net.Http.Json;
using Signature.API.Application.DTOs;

namespace Signature.Tests.IntegrationTests
{
    public class SistemaFixture : IAsyncLifetime
    {
        public HttpClient Client { get; private set; } = null!;
        public Guid CreatedUserId { get; private set; }

        public async Task InitializeAsync()
        {
            var factory = new SignatureApiFactory();
            factory.Server.AllowSynchronousIO = true;
            Client = factory.CreateClient();

            var userDto = new CreateUserDto("Marco", "marco@ex.com");
            var resp = await Client.PostAsJsonAsync("/api/users", userDto);
            resp.EnsureSuccessStatusCode();

            var created = await resp.Content.ReadFromJsonAsync<UserDto>()!;
            CreatedUserId = created.Id;
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}
