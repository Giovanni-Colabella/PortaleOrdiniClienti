using API.Models.Requests.Validators;
using API.Models.Services.Application;
using API.Services;
using FluentValidation;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Aggiungi Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GestioneFacile API",
        Version = "v1",
        Description = "API per la gestione di clienti e ordini.",
        Contact = new OpenApiContact
        {
            Name = "Giovanni Colabella",
            Email = "giovannicolabell@gmail.com",
        }
    });
});

builder.Services.AddControllers();

// Aggiungi FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

// Aggiungi DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer("name=DefaultConnection");
});

builder.Services.AddScoped<IClienteService, EfCoreClienteService>();
builder.Services.AddScoped<IOrdineService, EfCoreOrdineService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "GestioneFacile API ");
        options.RoutePrefix = string.Empty; // Per accedere a Swagger UI direttamente su / (root)
    });
}

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
