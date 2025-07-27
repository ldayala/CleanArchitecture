using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Api.OptionsSetup;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Abstractions.Authentication;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services serilog
//con esto decimos que la configuracion de serilog se va a leer desde el archivo appsettings.json
builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration));
Serilog.Debugging.SelfLog.Enable(Console.Error.WriteLine);


builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddTransient<IJwtProvider, JwtProvider>();

builder.Services.AddAuthorization();
// Add custom authorization policies
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAutorizacionHandler>();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizacionPolicyProvider>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();
/*
if (app.Environment.IsDevelopment()  )
{
   
}*/
app.UseSwagger();
app.UseSwaggerUI();

await app.ApplyMigration();
app.SeedData();
app.SeedAuthentication();

app.UseCustomExceptionHandler();
//configuramos el middleware creado por nosotros para que registre el contexto de la peticion
app.UseRequestContextLogging();

//configuramos el middleware de serilog para que registre las peticiones y respuestas
app.UseSerilogRequestLogging();

Log.Information("Prueba de conexión a Seq: la aplicación ha iniciado correctamente.");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

