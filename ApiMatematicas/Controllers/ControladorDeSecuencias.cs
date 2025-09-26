using ApiMatematicas.Clases;
using ApiMatematicas.Models;
using Microsoft.AspNetCore.Mvc;
using ApiMatematicas.Data;
using Microsoft.EntityFrameworkCore;
namespace ApiMatematicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorDeSecuencias : ControllerBase
    {
        private static readonly List<Secuencia> Secuencias = new()
    {
        new Secuencia { Id = 1, Numeros = new List<int>{2, 4, 6, 8}, Respuesta = 10, Dificultad="Facil" },
        new Secuencia { Id = 2, Numeros = new List<int>{3, 6, 12, 24}, Respuesta = 48, Dificultad="Facil" },
        // ...
    };

        [HttpPost("responder")]
        public async Task<IActionResult> Responder([FromBody] RespuestaUsuarioDto dto, [FromServices] AppDbContext db)
        {
            var secuencia = Secuencias.FirstOrDefault(s => s.Id == dto.SecuenciaId);
            if (secuencia == null) return NotFound(new { mensaje = "Secuencia no encontrada" });

            bool esCorrecta = dto.RespuestaUsuario == secuencia.Respuesta;

            if (dto.UsuarioId <= 0)
                return BadRequest(new { mensaje = "UserId es requerido en esta versión." });

            // Buscar por UserId + Modo
            var racha = await db.Rachas.FirstOrDefaultAsync(r => r.UsuarioId == dto.UsuarioId && r.Modo == dto.Modo);

            if (racha == null)
            {
                racha = new SistemaRacha
                {
                    UsuarioId = dto.UsuarioId,
                    Modo = dto.Modo,
                    Actual = 0,
                    Maxima = 0,
                    FechaUltimaActualizacion = DateTime.UtcNow
                };
                db.Rachas.Add(racha);
            }

            if (esCorrecta)
            {
                if (racha.Actual == 0)
                    racha.InicioRachaActual = DateTime.UtcNow;

                racha.Actual++;
                if (racha.Actual > racha.Maxima)
                    racha.Maxima = racha.Actual;
            }
            else
            {
                racha.Actual = 0;
                racha.InicioRachaActual = null;
            }

            racha.FechaUltimaActualizacion = DateTime.UtcNow;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Manejo simple de concurrencia/errores:
                return StatusCode(500, new { mensaje = "Error guardando racha", detalle = ex.Message });
            }

            return Ok(new
            {
                correcta = esCorrecta,
                respuestaCorrecta = secuencia.Respuesta,
                modo = dto.Modo.ToString(),
                rachaActual = racha.Actual,
                rachaMaxima = racha.Maxima,
                inicioRachaActual = racha.InicioRachaActual
            });
        }
    }
}
