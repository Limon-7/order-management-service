using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Platform.Infrastructure.Authentication;
using Platform.Infrastructure.Core.Extensions;
using Platform.Infrastructure.Core.Validation.Extensions;
using Platform.Infrastructure.Host.Contracts;
using Platform.Infrastructure.Host.WebApi.Extensions;
using Platform.Infrastructure.MassTransit.Bus.RabbitMQ.Extensions;
using Platform.Infrastructure.Repository.MongoDb;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ── Core platform services ─────────────────────────────────────────────
        builder.Services.AddCoreServices(new HostServiceConfig
        {
            UseEndPointMfaProtection = false,
            UseEndpointProtection = false,
        });

        builder.Services.AddMvc();

        // ── Read repository ────────────────────────────────────────────────────
        builder.Services.AddReadRepository();

        // ── Validation ─────────────────────────────────────────────────────────
        builder.Services.AddFluentValidation(options =>
        {
            options.ValidateAllCommands = false;
            options.ValidateAllQuery = false;
        });

        // ── Authentication ─────────────────────────────────────────────────────
        builder.Services.AddJwtBearer(builder.Configuration);

        // ── Message bus ───────────────────────────────────────────────────────
        builder.Services.AddInMemoryBusServices();
        builder.Services.AddServiceBusProvider(builder.Configuration);

        // ── Controllers & JSON ─────────────────────────────────────────────────
        builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = null
                };
            });

        builder.Services.Configure<ApiBehaviorOptions>(option =>
        {
            option.SuppressModelStateInvalidFilter = true;
        });

        // ── Swagger ────────────────────────────────────────────────────────────
        builder.Services.AddSwaggerGen();

        // ── Query handlers ─────────────────────────────────────────────────────
        // TODO: Register query handlers here
        // builder.Services.AddScoped<IQueryHandlerAsync<ExampleQuery, ExampleResponse>, ExampleQueryHandler>();

        // ── Build ──────────────────────────────────────────────────────────────
        var app = builder.Build();

        app.UseHttpPipeline(
            enableAuthorization: true,
            enableServiceIdCheckerMiddleware: false,
            enableTenantIdCheckerMiddleware: false);

        app.UseRouting();

        app.UseGlobalExceptionHandler();

        app.UseCors(corsPolicyBuilder =>
            corsPolicyBuilder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials()
                .SetPreflightMaxAge(TimeSpan.FromDays(365)));

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI();

        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("OrderManagement BusinessApiService started successfully.");

        app.Run();
    }
}
