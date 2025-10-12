using ApiMatematicas.Data;
using ApiMatematicas.Strategy;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit;
using NETCore.MailKit.Core;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;


var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Configuración de la DB
// -------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


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


// Registrar el proveedor
builder.Services.AddSingleton<IMailKitProvider>(new MailKitProvider(mailKitOptions));

// Registrar EmailService
builder.Services.AddScoped<IEmailService, EmailService>();

// Registrar recuperación de contraseña
builder.Services.AddScoped<IRecuperarContrasena, RecuperacionContrasena>();

// -------------------------
// Registrar tu estrategia
// -------------------------


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
