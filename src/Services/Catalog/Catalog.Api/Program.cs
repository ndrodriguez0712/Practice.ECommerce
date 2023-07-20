using Catalog.Persistence.Database;
using Catalog.Service.EventHandlers.Commands;
using Catalog.Service.Queries;
using Catalog.Service.Queries.Interfaces;
using Common.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseSqlServer(
        config.GetConnectionString("DefaultConnection"),
        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Catalog")
  )
);

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<ProductCreateCommand>());

builder.Services.AddTransient<IProductQueryService, ProductQueryService>();

// Health Checks Configurations.
builder.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddDbContextCheck<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();    
}


// Papertrail Configuration
var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
loggerFactory.AddSyslog(config.GetValue<string>("Papertrail:host"), config.GetValue<int>("Papertrail:port"));

app.UseAuthorization();

app.UseRouting()
   .UseEndpoints(config =>
    {
        // Health Checks Configurations.
        config.MapHealthChecks("/hc", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    });

app.MapControllers();

app.Run();
