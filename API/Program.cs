using System.Security.Claims;
using System.Text;

using API.Models.Entities;
using API.Models.Requests.Validators;
using API.Models.Services.Application;
using API.Models.Services.Infrastructure;
using API.Services;

using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

/****************************************
 *          Services Configuration      *
 ****************************************/
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger Configuration
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GestioneFacile API",
        Version = "v1",
        Description = "API per la gestione di clienti e ordini.",
        Contact = new OpenApiContact
        {
            Name = "Giovanni Colabella",
            Email = "giovannicolabell@gmail.com"
        }
    });

});

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Authentication Configuration (da usare quando si usa JWT )

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

builder.Services.AddAuthentication(options =>
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
        IssuerSigningKey = key, // Usa la chiave creata sopra
        RoleClaimType = ClaimTypes.Role, // Usa il tipo corretto
        ClockSkew = TimeSpan.Zero // Aggiungi questo
    };

    // Legge il token dal cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["jwtToken"];
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();


// Application Services
builder.Services.AddScoped<IClienteService, EfCoreClienteService>();
builder.Services.AddScoped<IOrdineService, EfCoreOrdineService>();
builder.Services.AddScoped<IProdottoService, EfCoreProdottoService>();
builder.Services.AddScoped<IImagePersister, ImageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<ITokenBlacklist, TokenBlackList>();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

/****************************************
 *          Middleware Pipeline         *
 ****************************************/
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "GestioneFacile API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors(policy => policy
    .WithOrigins("http://localhost:5100")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
); 

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Endpoint di benvenuto --> conduce su swagger
app.MapGet("/", () => Results.LocalRedirect("/swagger"));

app.Run();