

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Signature.API.Infra.Data.Context;
using Signature.API.Domain.Interfaces;
using Signature.API.Infra.Data.Repositories;
using Signature.API.Application.Interfaces;
using Signature.API.Application.Services;
using MassTransit;
using FluentMigrator.Runner;

namespace Signature.API.Infra.Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration config)
        {
            //services.AddDbContext<AppDbContext>(opt =>
            //    opt.UseNpgsql(config.GetConnectionString("DefaultConnection")));

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(
                        new Uri(config["RabbitMq:Host"]),
                        h =>
                        {
                            h.Username(config["RabbitMq:Username"]);
                            h.Password(config["RabbitMq:Password"]);
                        });
                });
            });

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(config.GetConnectionString("DefaultConnection"))
                    .ScanIn(typeof(Signature.API.Infra.Data.Migrations.InitialMigration).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
            services.AddScoped<IUserSubscriptionRepository, UserSubscriptionRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();

            return services;
        }
    }
}
