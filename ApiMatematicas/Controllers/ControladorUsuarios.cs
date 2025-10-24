using ApiMatematicas.Data;
using ApiMatematicas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiMatematicas.Utilidades;
using System.Security.Cryptography;
using System.Text;
using ApiMatematicas.Strategy.InicioSesionStrateg;


namespace ApiMatematicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorUsuarios : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IRecuperarTokenInicioStrategy _recuperarToken;


        public ControladorUsuarios(AppDbContext context, IConfiguration config, IRecuperarTokenInicioStrategy recupearToken)
        {
            _context = context;
            _config = config;
            _recuperarToken = recupearToken;
        }

        [HttpPost("Registro")]
        public async Task<IActionResult> Registro([FromBody] Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.nombreUsuario) || string.IsNullOrEmpty(usuario.contrasena))
                return BadRequest(new { mensaje = "El nombre de usuario y la contraseña son obligatorios." });

            if (await _context.Usuarios.AnyAsync(u => u.nombreUsuario == usuario.nombreUsuario))
                return BadRequest(new { mensaje = "El usuario ya existe." });

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.contrasena);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario registrado correctamente" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Usuario login)
        {
            if (string.IsNullOrEmpty(login.nombreUsuario) || string.IsNullOrEmpty(login.contrasena))
                return BadRequest(new { mensaje = "El nombre de usuario y la contraseña son obligatorios." });

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.nombreUsuario == login.nombreUsuario);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.contrasena, usuario.PasswordHash))
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrecta." });

            // Generar JWT
            var token = _recuperarToken.GenerateToken(usuario);

            return Ok(new { token, nombreUsuario = usuario.nombreUsuario });
        }
    }
}
