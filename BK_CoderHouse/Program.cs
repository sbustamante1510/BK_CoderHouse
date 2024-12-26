using BK_CoderHouse.Application.Interfaces;
using BK_CoderHouse.Application.Services;
using BK_CoderHouse.Middlewares;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ITiendaService, TiendaService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen((options) =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

var app = builder.Build();


app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"Suizasoft Integracion Siteds v1 - {app.Environment.EnvironmentName}");
    c.InjectStylesheet("css/SwaggerDark.css"); // Ruta al archivo CSS personalizado
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseCors(x => x
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .SetIsOriginAllowed(origin => true)
                  .AllowCredentials());

app.UseAuthentication(); // Asegurate de que UseAuthentication va antes de UseAuthorization
app.UseMiddleware<ValidacionTokenPersonalizadaMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
