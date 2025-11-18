using ApiMatematicas.Clases;
using ApiMatematicas.Data;
using ApiMatematicas.DTO;
using ApiMatematicas.Models;
using ApiMatematicas.Strategy.AbstaccionStrategy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMatematicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorAbstraccion : ControllerBase
    {
        private readonly AbstraccionStrategy _strategy;
        private readonly AppDbContext _db;

        public ControladorAbstraccion(AppDbContext db)
        {
            _strategy = new AbstraccionStrategy();
            _db = db;
        }

        // Función opcional para decidir dificultad según racha
        private string ObtenerDificultad(int rachaActual, int rachaMaxima)
        {
            double rendimiento = (rachaActual * 0.6 + rachaMaxima * 0.4);

            if (rendimiento >= 50)
                return "Dificil";
            else if (rendimiento >= 25)
                return "Medio";
            else
                return "Facil";
        }

        // GET: Obtener pregunta adaptativa para el usuario
        [HttpGet("obtener/{usuarioId}/{modo}")]
        public async Task<IActionResult> ObtenerPregunta(int usuarioId, int modo)
        {
            if (usuarioId <= 0)
                return BadRequest(new { mensaje = "UsuarioId inválido" });

            // Recuperar racha actual y máxima del usuario
            var racha = await _db.Rachas.FirstOrDefaultAsync(r => r.usuarioId == usuarioId && r.modo == (ModoJuego)modo);
            int rachaActual = racha?.actual ?? 0;
            int rachaMaxima = racha?.maxima ?? 0;

            // Decidir dificultad si lo quieres implementar
            string dificultad = ObtenerDificultad(rachaActual, rachaMaxima);

            // Generar pregunta usando Strategy
            var pregunta = _strategy.GenerarPregunta(usuarioId);

            if (pregunta == null) return NotFound(new { mensaje = "No hay preguntas disponibles" });

            return Ok(new
            {
                preguntaId = pregunta.Id,
                titulo = pregunta.Titulo,
                imagen = pregunta.Imagen,
                descripcion = pregunta.Descripcion,
                opciones = pregunta.Opciones,        
                modo = (ModoJuego)modo,
                rachaActual,
                rachaMaxima
            });
        }

        // POST: Responder pregunta
        [HttpPost("responder")]
        public async Task<IActionResult> Responder([FromBody] RespuestaUsuarioAbstraccionDTO dto)
        {
            if (dto.UsuarioId <= 0)
                return BadRequest(new { mensaje = "UsuarioId es requerido" });

            // Buscar o crear racha del usuario
            var racha = await _db.Rachas.FirstOrDefaultAsync(r => r.usuarioId == dto.UsuarioId && r.modo == dto.Modo);
            if (racha == null)
            {
                racha = new SistemaRacha
                {
                    usuarioId = dto.UsuarioId,
                    modo = dto.Modo,
                    actual = 0,
                    maxima = 0,
                    fechaUltimaActualizacion = DateTime.UtcNow
                };
                _db.Rachas.Add(racha);
            }

            // Recuperar pregunta para evaluar
            var pregunta = _strategy.ObtenerPreguntaPorId(dto.PreguntaId);
            if (pregunta == null) return NotFound(new { mensaje = "Pregunta no encontrada" });

            bool esCorrecta = _strategy.EvaluarRespuesta(dto.UsuarioId, pregunta.Id, dto.RespuestaUsuario);

            // Actualizar racha
            if (esCorrecta)
            {
                if (racha.actual == 0) racha.inicioRachaActual = DateTime.UtcNow;
                racha.actual++;
                if (racha.actual > racha.maxima) racha.maxima = racha.actual;
            }
            else
            {
                racha.actual = 0;
                racha.inicioRachaActual = null;
            }

            racha.fechaUltimaActualizacion = DateTime.UtcNow;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensaje = "Error guardando racha", detalle = ex.Message });
            }

            return Ok(new
            {
                correcta = esCorrecta,
                respuestaCorrecta = pregunta.RespuestaCorrecta,
                modo = dto.Modo.ToString(),
                rachaActual = racha.actual,
                rachaMaxima = racha.maxima,
                inicioRachaActual = racha.inicioRachaActual
            });
        }
    }
}
