using Common.Logging;
using Customer.Persistence.Database;
using Customer.Service.EventHandlers.Commands;
using Customer.Service.Queries.Interfaces;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Order.Service.Queries;

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
        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Customer")
  )
);

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<ClientCreateCommand>());

builder.Services.AddTransient<IClientQueryService, ClientQueryService>();

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
