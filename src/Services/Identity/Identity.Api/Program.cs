using Common.Logging;
using HealthChecks.UI.Client;
using Identity.Api.Extensions;
using Identity.Persistence.Database;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

System.Globalization.CultureInfo.CurrentCulture = new CultureInfo("en-US");

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .Build();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOptions(configuration);
builder.Services.AddDbContexts(configuration);
builder.Services.AddServices();
builder.Services.AddSwagger($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

// Health Checks Configurations.
builder.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddDbContextCheck<ApplicationDbContext>();

// ApiUrls
// builder.Services.Configure<ApiUrls>(opts => config.GetSection("ApiUrls").Bind(opts));

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
loggerFactory.AddSyslog(configuration.GetValue<string>("Papertrail:host"), configuration.GetValue<int>("Papertrail:port"));

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
