using Identity.Persistence.Database.DataAccess;
using Identity.Persistence.Database.Interfaces;
using Identity.Service.EventHandlers.Helpers.Interfaces;
using Identity.Service.EventHandlers.Helpers;
using Identity.Service.Queries.DTOs;
using Identity.Service.Queries.Interfaces;
using Identity.Service.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Identity.Persistence.Database;
using Identity.Service.EventHandlers.Commands;

namespace Identity.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "User")), ServiceLifetime.Transient);

            return services;
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpConfigurationDto>(configuration.GetSection("SmtpConfigurations"));

            services.Configure<EncryptionOptionsDto>(configuration.GetSection("EncryptionOptions"));

            services.Configure<CaptchaConfigurationDto>(configuration.GetSection("Captcha"));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork<ApplicationDbContext>, UnitOfWork<ApplicationDbContext>>();
            services.AddTransient<IUserQueryService, UserQueryService>();
            services.AddSingleton<IUserAuthManager, UserAuthManager>();

            services.AddScoped<ICaptchaManager, CaptchaManager>();
            services.AddScoped<IEmailSenderManager, EmailSenderManager>();
            services.AddScoped<IEncryptionManager, EncryptionManager>();

            services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<UserCreateCommand>());
            services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<UserLoginCommand>());

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, string xmlFileName)
        {
            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Practice ECommerce API", Version = "v0.01" });

                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                doc.IncludeXmlComments(xmlPath);
                doc.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please, enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                doc.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            return services;
        }
    }
}
