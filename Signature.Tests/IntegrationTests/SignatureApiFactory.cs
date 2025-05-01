using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Signature.API.Infra.Data.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Signature.Tests.IntegrationTests
{
    public class SignatureApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Integration");

            builder.ConfigureServices(services =>
            {
                // 1) Remove qualquer DbContextOptions<AppDbContext> registrado
                var optionsDescriptors = services
                    .Where(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>))
                    .ToList();
                foreach (var d in optionsDescriptors)
                    services.Remove(d);

                // 2) Remove qualquer inscrição direta de AppDbContext
                var contextDescriptors = services
                    .Where(d => d.ServiceType == typeof(AppDbContext))
                    .ToList();
                foreach (var d in contextDescriptors)
                    services.Remove(d);

                // 3) Registra somente o InMemory para AppDbContext
                services.AddDbContext<AppDbContext>(opts =>
                    opts.UseInMemoryDatabase("TestDb"));

                // 4) Reconstrói o provedor e (re)popula dados iniciais
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Garante banco limpo a cada inicialização
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();

                // Seed básico
                ctx.SubscriptionPlans.Add(new Signature.API.Domain.Entities.SubscriptionPlan("PlanoTeste", 10m));
                ctx.SaveChanges();
            });
        }
    }
}
