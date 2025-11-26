using ApiMatematicas.Data;
using ApiMatematicas.Strategy.InicioSesionStrateg;
using ApiMatematicas.Strategy.InicioSesionStrategy;
using ApiMatematicas.Strategy.RecupearContraseñaStrategy;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit;
using NETCore.MailKit.Core;
using NETCore.MailKit.Infrastructure.Internal;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// DB
var supabasePassword = builder.Configuration["SUPABASE_PASSWORD"];
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!
    .Replace("PLACEHOLDER", supabasePassword);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// JWT
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new Exception("JWT Key no encontrada en configuración.");

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Controllers + JSON
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MailKit
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

builder.Services.AddSingleton<IMailKitProvider>(new MailKitProvider(mailKitOptions));
builder.Services.AddScoped<IEmailService, EmailService>();

// Recuperación de contraseña / login
builder.Services.AddScoped<IRecuperarContrasena, RecuperacionContrasena>();
builder.Services.AddScoped<IRecuperarTokenInicioStrategy, JwtTokenStrategy>();

// Build app
var app = builder.Build();

// Static files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Imagenes")),
    RequestPath = "/Imagenes"
});

// Middleware
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Run
app.Run();
