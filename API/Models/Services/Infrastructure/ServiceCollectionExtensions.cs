using System.IdentityModel.Tokens.Jwt;
using System.Text;

using API.Models.Entities;
using API.Models.Services.Application;
using API.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API.Models.Services.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API",
                Version = "v1",
                Description = "API per la gestione di clienti e ordini.",
                Contact = new OpenApiContact
                {
                    Name = "Giovanni Colabella",
                    Email = "giovannicolabell@gmail.com"
                }
            });

        });
        return services;
    }
    public static IServiceCollection AddCorsWithDefaultValues(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", builder =>
                builder.WithOrigins("http://localhost:5100")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed(_ => true));
        });
        return services;
    }
    public static IServiceCollection AddDbContextDefault(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        return services;
    }
    public static IServiceCollection AddIdentityDefault(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var jwtSettings = config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = key,
            };

            // options.Events = new JwtBearerEvents
            // {
            //     // Legge il token dal cookie
            //     OnMessageReceived = context =>
            //     {
            //         context.Token = context.Request.Cookies["jwtToken"];
            //         return Task.CompletedTask;
            //     },

            //     // questo evento controlla se il token è presente nella blacklist o meno

            //     OnTokenValidated = context =>
            //     {
            //         if (context.SecurityToken is not JwtSecurityToken jwtToken)
            //         {
            //             context.Fail("Token non valido");
            //             return Task.CompletedTask;
            //         }

            //         var tokenString = jwtToken.RawData;
            //         var blacklist = context.HttpContext
            //                                 .RequestServices
            //                                 .GetRequiredService<ITokenBlacklist>();

            //         if (blacklist.IsRevoked(tokenString))
            //             context.Fail("Token revocato");

            //         return Task.CompletedTask;
            //     },
            //     OnAuthenticationFailed = context =>
            //     {
            //         Console.WriteLine($"Errore di autenticazione: {context.Exception.Message}");
            //         return Task.CompletedTask;
            //     }

            // };
        });

        return services;
    }
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IClienteService, EfCoreClienteService>();
        services.AddScoped<IOrdineService, EfCoreOrdineService>();
        services.AddScoped<IProdottoService, EfCoreProdottoService>();
        services.AddScoped<IImagePersister, ImageService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<ITokenBlacklist, TokenBlacklist>();
        services.AddScoped<IUtenteBloccatoService, BanUserByEmailService>();

        return services;
    }

}
