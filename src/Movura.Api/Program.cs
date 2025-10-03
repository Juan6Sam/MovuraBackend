using Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Movura.Api.Middleware;
using Movura.Api.Data.Context;
using Movura.Api.Services;
using Movura.Api.Services.Auth;
using Movura.Api.Services.Interfaces;

// Configuración inicial de Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Iniciando la aplicación Movura API...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Usar Serilog para el logging
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // 1. CONFIGURACIÓN DE SERVICIOS

    builder.Services.AddControllers();

    // DbContext
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<MovuraDbContext>(options =>
        options.UseSqlServer(connectionString));

    // AutoMapper
    builder.Services.AddAutoMapper(typeof(Program));

    // --- Registro de Servicios Personalizados ---
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<JwtHelper>();
    builder.Services.AddScoped<ReportService>();
    builder.Services.AddScoped<IEmailService, EmailService>();

    // Autenticación JWT
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                ValidAudience = builder.Configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
            };
        });

    // Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // CORS
    var corsOrigins = builder.Configuration.GetSection("CORS:Origins").Get<string[]>() ?? new string[] { };
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins",
            policy =>
            {
                policy.WithOrigins(corsOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
    });

    // 2. CONSTRUCCIÓN Y CONFIGURACIÓN DEL PIPELINE

    var app = builder.Build();

    // Verificación de la conexión a la base de datos
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MovuraDbContext>();
        Log.Information("Verificando conexión con la base de datos...");
        try
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                Log.Information("✅ Conexión con la base de datos establecida correctamente.");
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "❌ No se pudo establecer conexión con la base de datos.");
        }
    }

    // Pipeline de peticiones HTTP
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseMiddleware<RequestLoggingMiddleware>();
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowSpecificOrigins");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // 3. EJECUCIÓN
    app.Run();
}
catch (Exception ex)
{
    // Asegurarse de que el logger capture la excepción completa
    Log.Fatal(ex, "La aplicación ha fallado al iniciar. Exception: {ExceptionObject}", ex);
}
finally
{
    Log.Information("Cerrando la aplicación.");
    Log.CloseAndFlush();
}
