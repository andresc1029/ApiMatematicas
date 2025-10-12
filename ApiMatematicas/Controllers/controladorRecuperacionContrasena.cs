using ApiMatematicas.Data;
using ApiMatematicas.DTO;
using ApiMatematicas.Strategy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMatematicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorRecuperacionContrasena : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRecuperarContrasena _recuperarContrasena;

        public ControladorRecuperacionContrasena(AppDbContext context, IRecuperarContrasena recuperarContrasena)
        {
            _context = context;
            _recuperarContrasena = recuperarContrasena;
        }

        // ------------------ Enviar código de recuperación ------------------
        [HttpPost("EnviarCodigo")]
        public async Task<IActionResult> EnviarCodigo([FromBody] SolicitudContraseñaDTO dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == dto.CorreoUsuario);
            if (usuario == null)
                return BadRequest(new { mensaje = "El correo no está registrado." });

            try
            {
                await _recuperarContrasena.EnviarCodigoRecuperacionAsync(dto.CorreoUsuario);
                return Ok(new { mensaje = "Código de recuperación enviado al correo." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // ------------------ Restablecer contraseña ------------------
        [HttpPost("Restablecer")]
        public async Task<IActionResult> Restablecer([FromBody] CambioContraseñaDTO dto)
        {
            try
            {
                var resultado = await _recuperarContrasena.RestablecerContraseñaAsync(
                    dto.CorreoUsuario,
                    dto.Token,
                    dto.NuevaContraseña
                );

                return Ok(new { mensaje = "Contraseña restablecida correctamente", exito = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}