using ApiMatematicas.Clases;
using ApiMatematicas.Models;
using ApiMatematicas.Strategy.ProblemasCondicionesStrategy;
using Microsoft.AspNetCore.Mvc;
using ApiMatematicas.Data;
using Microsoft.EntityFrameworkCore;
using ApiMatematicas.DTO;

namespace ApiMatematicas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorProblemasCondiciones : ControllerBase
    {
        private readonly CondicionesStrategy _strategy;
        private readonly AppDbContext _db;

        public ControladorProblemasCondiciones(AppDbContext db)
        {
            _strategy = new CondicionesStrategy();
            _db = db;
        }

        // Función para decidir dificultad según racha máxima e histórica
        private string ObtenerDificultad(int rachaActual, int rachaMaxima)
        {
            // Promedio ponderado entre desempeño actual y máximo histórico
            double rendimiento = (rachaActual * 0.6 + rachaMaxima * 0.4);

            if (rendimiento >= 50)
                return "Dificil";
            else if (rendimiento >= 25)
                return "Medio";
            else
                return "Facil";
        }


        // GET: Obtener problema adaptativo para el usuario
        [HttpGet("obtener/{usuarioId}/{modo}")]
        public async Task<IActionResult> ObtenerProblema(int usuarioId, int modo)
        {
            // Recuperar racha actual y máxima del usuario
            var racha = await _db.Rachas.FirstOrDefaultAsync(r => r.UsuarioId == usuarioId && r.Modo == (ModoJuego)modo);
            int rachaActual = racha?.Actual ?? 0;
            int rachaMaxima = racha?.Maxima ?? 0;

            // Decidir dificultad
            string dificultad = ObtenerDificultad(rachaActual, rachaMaxima);

            // Generar problema usando Strategy
            var problema = _strategy.GenerarProblema(dificultad);

            if (problema == null) return NotFound(new { mensaje = "No hay problemas disponibles" });

            // Enviar problema al frontend sin revelar la respuesta
            return Ok(new
            {
                problemaId = problema.Id,
                enunciado = problema.Enunciado,
                opciones = new[] {
                    problema.OpcionA,
                    problema.OpcionB,
                    problema.OpcionC,
                    problema.OpcionD 
                }.Where(o => o != null),
                dificultad,
                modo = (ModoJuego)modo,
                rachaActual,
                rachaMaxima
            });
        }

        // POST: Responder problema
        [HttpPost("responder")]
        public async Task<IActionResult> Responder([FromBody] RespuestaUsuarioCondicionesDTO dto)
        {
            if (dto.UsuarioId <= 0)
                return BadRequest(new { mensaje = "UserId es requerido" });

            // Buscar o crear racha del usuario
            var racha = await _db.Rachas.FirstOrDefaultAsync(r => r.UsuarioId == dto.UsuarioId && r.Modo == dto.Modo);
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
                _db.Rachas.Add(racha);
            }

            // Recuperar problema para evaluar
            var problema = _strategy.ObtenerProblemaPorId(dto.ProblemaId);
            if (problema == null) return NotFound(new { mensaje = "Problema no encontrado" });

            bool esCorrecta = _strategy.EvaluarRespuesta(problema.Id, dto.RespuestaUsuario.ToString());

            // Actualizar racha
            if (esCorrecta)
            {
                if (racha.Actual == 0) racha.InicioRachaActual = DateTime.UtcNow;
                racha.Actual++;
                if (racha.Actual > racha.Maxima) racha.Maxima = racha.Actual;
            }
            else
            {
                racha.Actual = 0;
                racha.InicioRachaActual = null;
            }

            racha.FechaUltimaActualizacion = DateTime.UtcNow;

            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { mensaje = "Error guardando racha", detalle = ex.Message });
            }

            return Ok(new
            {
                correcta = esCorrecta,
                modo = dto.Modo.ToString(),
                rachaActual = racha.Actual,
                rachaMaxima = racha.Maxima,
                inicioRachaActual = racha.InicioRachaActual
            });
        }
    }
}
