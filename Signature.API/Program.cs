using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Signature.API.Infra.Data.Context;
using Signature.API.Infra.Ioc;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (builder.Environment.IsEnvironment("Integration"))
        {
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseInMemoryDatabase("TestDb"));
        }
        else
        {
            builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")));
        }

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        using (var scope = app.Services.CreateScope())
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }
}

public partial class Program { }