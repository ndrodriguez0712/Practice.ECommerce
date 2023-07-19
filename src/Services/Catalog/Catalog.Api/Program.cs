using Catalog.Persistence.Database;
using Catalog.Service.EventHandlers.Commands;
using Catalog.Service.Queries;
using Catalog.Service.Queries.Interfaces;
using Common.Logging;
using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();    
}

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
loggerFactory.AddSyslog(config.GetValue<string>("Papertrail:host"), config.GetValue<int>("Papertrail:port"));

app.UseAuthorization();

app.MapControllers();

app.Run();
