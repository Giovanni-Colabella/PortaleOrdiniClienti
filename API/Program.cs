using API.Models.Requests.Validators;
using API.Models.Services.Application;
using API.Services;
using FluentValidation;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Aggiungi FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

// Aggiungi DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer("name=DefaultConnection");
});

builder.Services.AddTransient<IClienteService, EfCoreClienteService>();
builder.Services.AddTransient<IOrdineService, EfCoreOrdineService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
