
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Movura.Api.Data.Context;
using Movura.Api.Middleware;
using Movura.Api.Services;
using Movura.Api.Services.Auth;
using Movura.Api.Services.Interfaces;
using Serilog;
using FluentValidation.AspNetCore;
using System.IO;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movura Admin API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// Configurar Base de Datos con SQL Server
builder.Services.AddDbContext<MovuraDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("CORS:Origins").Get<string[]>() ?? Array.Empty<string>())
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Configurar Autenticación JWT y Autorización
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? throw new InvalidOperationException())),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(AuthorizationPolicies.ConfigurePolicies);

// Configurar AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configurar servicios de la aplicación
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IParkingService, ParkingService>();
builder.Services.AddScoped<IComercioService, ComercioService>();
builder.Services.AddScoped<IReportService, ReportService>();

// Agregar servicio de validación
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// =======================================================================================
// INICIO: MÓDULO DE VERIFICACIÓN DE CONEXIÓN A LA BASE DE DATOS
// Este bloque verifica la conexión a la base de datos al iniciar la aplicación.
// No es necesario comentarlo en producción, ya que es una verificación ligera.
// =======================================================================================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MovuraDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Verificando conexión con la base de datos...");
        var canConnect = await dbContext.Database.CanConnectAsync();
        if (canConnect)
        {
            logger.LogInformation("✅ Conexión con la base de datos establecida correctamente.");
        }
        else
        {
            logger.LogCritical("❌ Error Crítico: No se pudo establecer conexión con la base de datos.");
        }
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "❌ Error Crítico: Excepción al intentar conectar con la base de datos.");
    }
}
// =======================================================================================
// FIN: MÓDULO DE VERIFICACIÓN DE CONEXIÓN A LA BASE DE DATOS
// =======================================================================================


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // =======================================================================================
    // INICIO: MIDDLEWARE DE LOGGING DE PETICIONES (SOLO DESARROLLO)
    // Este middleware registra el cuerpo de las peticiones y respuestas.
    // Para producción, simplemente comenta o elimina la siguiente línea.
    // =======================================================================================
    app.UseMiddleware<RequestLoggingMiddleware>();
    // =======================================================================================
    // FIN: MIDDLEWARE DE LOGGING DE PETICIONES
    // =======================================================================================
}

// Configurar middleware de excepciones personalizado
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
