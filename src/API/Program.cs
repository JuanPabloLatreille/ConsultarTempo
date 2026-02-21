using API.Middlewares;
using Infraestructure.Data.Context;
using Infraestructure.DependencyInjections;
using Microsoft.EntityFrameworkCore;

namespace API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddApplication();
        builder.Services.AddRepositories();
        builder.Services.AddPersistence(builder.Configuration);
        builder.Services.AddServices(builder.Configuration);
        builder.Services.AddHealthChecks();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseExceptionHandlerMiddleware();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHealthChecks("/health");

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            await context.Database.MigrateAsync();
        }

        await app.RunAsync();
    }
}