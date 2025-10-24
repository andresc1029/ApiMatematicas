using ApiMatematicas.Data;
using ApiMatematicas.Strategy.InicioSesionStrateg;
using ApiMatematicas.Strategy.InicioSesionStrategy;
using ApiMatematicas.Strategy.RecupearContraseñaStrategy;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit;
using NETCore.MailKit.Core;
using NETCore.MailKit.Infrastructure.Internal;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Configuración de la DB segura con user-secrets
// -------------------------
var supabasePassword = builder.Configuration["SUPABASE_PASSWORD"];

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!
    .Replace("PLACEHOLDER", supabasePassword);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
//JWT Secrets
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new Exception("JWT Key no encontrada en configuración.");


// -------------------------
// CORS
// -------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// -------------------------
// Controladores y Swagger
// -------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -------------------------
// MailKit (IEmailService registrado automáticamente)
// -------------------------
builder.Services.AddScoped<IEmailService, EmailService>();

var mailKitOptions = new MailKitOptions
{
    Server = "smtp.gmail.com",
    Port = 587,
    SenderName = "LaCupula App",
    SenderEmail = "lacupulabot@gmail.com",
    Account = "lacupulabot@gmail.com",
    Password = "pnfi vjtt pkny gwet",
    Security = true
};
// No tocar

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});


// Registrar el proveedor
builder.Services.AddSingleton<IMailKitProvider>(new MailKitProvider(mailKitOptions));

// Registrar EmailService
builder.Services.AddScoped<IEmailService, EmailService>();

// Registrar/ inicio/recuperación de contraseña
builder.Services.AddScoped<IRecuperarContrasena, RecuperacionContrasena>();
builder.Services.AddScoped<IRecuperarTokenInicioStrategy, JwtTokenStrategy>();

// -------------------------
// Construir app
// -------------------------
var app = builder.Build();

// -------------------------
// Middleware
// -------------------------
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// -------------------------
// Ejecutar app
// -------------------------
app.Run();
