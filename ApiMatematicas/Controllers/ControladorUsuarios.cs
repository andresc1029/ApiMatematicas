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


namespace ApiMatematicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorUsuarios : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public ControladorUsuarios(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
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

            if (!BCrypt.Net.BCrypt.Verify(login.contrasena, usuario.PasswordHash))
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrecta." });

            // Generar JWT
            var token = GenerateJwtToken(usuario);

            return Ok(new { token, nombreUsuario = usuario.nombreUsuario });
        }

        // ---- Métodos de ayuda ----
        

        
        private string GenerateJwtToken(Usuario usuario)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var duration = double.Parse(jwtSettings["DurationInMinutes"]);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.nombreUsuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", usuario.id.ToString()),
                new Claim("Correo", usuario.Correo ?? "")
            };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(duration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}