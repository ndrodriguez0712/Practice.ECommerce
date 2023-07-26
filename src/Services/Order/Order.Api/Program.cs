using Common.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Order.Persistence.Database;
using Order.Service.EventHandlers.Commands;
using Order.Service.Proxies.Catalog;
using Order.Service.Proxies.Interfaces;
using Order.Service.Queries;
using Order.Service.Queries.Interfaces;
using ServiceBusProvider;
using ServiceBusProvider.Interfaces;

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
        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Order")
  )
);

// Event handlers.
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<OrderCreateCommand>());

// Azure Service Bus .
builder.Services.AddSingleton<IServiceBusQueue, ServiceBusQueue>();

// Proxy.
builder.Services.AddTransient<ICatalogProxy, CatalogProxy>();

// Query services.
builder.Services.AddTransient<IOrderQueryService, OrderQueryService>();

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


// Papertrail Configuration.
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
