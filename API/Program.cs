using API.Models.Requests.Validators;
using API.Models.Services.Infrastructure;
using API.Models.Services.Infrastructure.Middlewares;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

/****************************************
 *          Services Configuration      *
 ****************************************/
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configura Swagger 
builder.Services.AddSwagger();

// Configura CORS 
builder.Services.AddCorsWithDefaultValues();

// configura Database 
builder.Services.AddDbContextDefault(builder.Configuration);

// configura Identity 
builder.Services.AddIdentityDefault();

// configura autenticazione
builder.Services.AddJwtAuthentication(builder.Configuration);

// configura autorizzazione 
builder.Services.AddAuthorization();

// Application Services
builder.Services.AddApplicationServices();

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

// Questo middleware ( si trova in models >> services >> infrastructure >> middlewares) controlla se l'ip del client è bannato
app.UseMiddleware<BannedIpMiddleware>();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Endpoint di benvenuto --> conduce su swagger
app.MapGet("/", () => Results.LocalRedirect("/swagger"));


app.Run();