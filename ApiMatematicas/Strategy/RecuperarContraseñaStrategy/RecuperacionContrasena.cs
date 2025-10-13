using ApiMatematicas.Data;
using ApiMatematicas.Models;
using ApiMatematicas.Utilidades;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;
using Org.BouncyCastle.Crypto.Generators;

namespace ApiMatematicas.Strategy.RecupearContraseñaStrategy
{
    public class RecuperacionContrasena : IRecuperarContrasena
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public RecuperacionContrasena(AppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

   
        public async Task EnviarCodigoRecuperacionAsync(string correo)
        {
            // Buscar usuario por correo
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
            if (user == null)
                throw new Exception("El correo no está registrado.");

            // Generar token
            var token = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            var expiration = DateTime.UtcNow.AddMinutes(15);

            // Guardar token en BD (ligado al usuario)
            var nuevoToken = new ReinicioContrasenaToken
            {
                UserId = user.id,
                Token = token,
                Expiration = expiration
            };

            _context.ReinicioContraseñas.Add(nuevoToken);
            await _context.SaveChangesAsync();

            // Enviar correo
            await _emailService.SendAsync(
                user.Correo,
                "Recuperación de contraseña",
                $"Tu código de recuperación es: {token}. Válido por 15 minutos.",
                isHtml: false
            );
        }

        public async Task<bool> ValidarCodigoAsync(string correo, string token)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
            if (usuario == null) return false;

            var codigo = await _context.ReinicioContraseñas
                .Where(r => r.UserId == usuario.id && r.Token == token)
                .OrderByDescending(r => r.Expiration)
                .FirstOrDefaultAsync();

            if (codigo == null) return false;
            if (codigo.Expiration < DateTime.UtcNow) return false;

            return true;
        }


        public Task<bool> RestablecerContraseña(string correo, string token, string codigo)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RestablecerContraseñaAsync(string email, string token, string nuevaContraseña)
        {
            // Buscar usuario
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == email);
            if (user == null)
                throw new Exception("Usuario no encontrado.");

            // Buscar el token asociado a ese usuario
            var resetToken = await _context.ReinicioContraseñas
                .FirstOrDefaultAsync(t => t.UserId == user.id && t.Token == token);

            if (resetToken == null)
                throw new Exception("Token inválido.");

            if (resetToken.Expiration < DateTime.UtcNow)
                throw new Exception("El token ha expirado.");

            // Actualizar la contraseña (encriptada)
            user.PasswordHash = PasswordHelper.HashPassword(nuevaContraseña);

            _context.Entry(user).State = EntityState.Modified;

            // Eliminar el token para que no se reutilice
            _context.ReinicioContraseñas.Remove(resetToken);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
