using Common.Logging;
using HealthChecks.UI.Client;
using Identity.Persistence.Database;
using Identity.Persistence.Database.DataAccess;
using Identity.Persistence.Database.Interfaces;
using Identity.Service.EventHandlers.Commands;
using Identity.Service.EventHandlers.Helpers;
using Identity.Service.EventHandlers.Helpers.Interfaces;
using Identity.Service.Queries;
using Identity.Service.Queries.Interfaces;
using k8s.KubeConfigModels;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

System.Globalization.CultureInfo.CurrentCulture = new CultureInfo("en-US");

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
        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "User")
  )
);

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<UserCreateCommand>());
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<UserLoginCommand>());

builder.Services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();
builder.Services.AddTransient<IUserQueryService, UserQueryService>();
builder.Services.AddSingleton<IUserAuthManager, UserAuthManager>();

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
