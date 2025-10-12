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
        // Secuencias por dificultad
        private static readonly List<Secuencia> SecuenciasFacil = new()
        {
             new Secuencia { Id = 1,  Numeros =  new List<int>{2,4,6,8}, Respuesta = 10, Dificultad="Facil" },
             new Secuencia { Id = 2,  Numeros =  new List<int>{3,6,12,24}, Respuesta = 48, Dificultad="Facil" },
             new Secuencia { Id = 3,  Numeros =  new List<int>{5,10,15,20}, Respuesta = 25, Dificultad="Facil" },
             new Secuencia { Id = 4,  Numeros =  new List<int>{1,2,3,4}, Respuesta = 5, Dificultad="Facil" },
             new Secuencia { Id = 5,  Numeros =  new List<int>{2,6,10,14}, Respuesta = 18, Dificultad="Facil" },
             new Secuencia { Id = 6,  Numeros =  new List<int>{0,1,2,3}, Respuesta = 4, Dificultad="Facil" },
             new Secuencia { Id = 7,  Numeros =  new List<int>{1,3,5,7}, Respuesta = 9, Dificultad="Facil" },
             new Secuencia { Id = 8,  Numeros =  new List<int>{10,20,30,40}, Respuesta = 50, Dificultad="Facil" },
             new Secuencia { Id = 9,  Numeros =  new List<int>{100,90,80,70}, Respuesta = 60, Dificultad="Facil" },
             new Secuencia { Id = 10, Numeros = new List<int>{7,14,21,28}, Respuesta = 35, Dificultad="Facil" },
             new Secuencia { Id = 11, Numeros = new List<int>{1,4,7,10}, Respuesta = 13, Dificultad="Facil" },
             new Secuencia { Id = 12, Numeros = new List<int>{8,16,24,32}, Respuesta = 40, Dificultad="Facil" },
             new Secuencia { Id = 13, Numeros = new List<int>{9,18,27,36}, Respuesta = 45, Dificultad="Facil" },
             new Secuencia { Id = 14, Numeros = new List<int>{50,45,40,35}, Respuesta = 30, Dificultad="Facil" },
             new Secuencia { Id = 15, Numeros = new List<int>{11,22,33,44}, Respuesta = 55, Dificultad="Facil" }
        };

        private static readonly List<Secuencia> SecuenciasMedio = new()
        {
            new Secuencia { Id = 16, Numeros = new List<int>{2,4,8,16,32}, Respuesta = 64, Dificultad="Medio" },
            new Secuencia { Id = 17, Numeros = new List<int>{1,2,4,7,11}, Respuesta = 16, Dificultad="Medio" },
            new Secuencia { Id = 18, Numeros = new List<int>{1,1,2,3,5}, Respuesta = 8, Dificultad="Medio" },
            new Secuencia { Id = 19, Numeros = new List<int>{1,4,9,16,25}, Respuesta = 36, Dificultad="Medio" },
            new Secuencia { Id = 20, Numeros = new List<int>{2,3,5,8,12}, Respuesta = 17, Dificultad="Medio" },
            new Secuencia { Id = 21, Numeros = new List<int>{3,6,12,24,48}, Respuesta = 96, Dificultad="Medio" },
            new Secuencia { Id = 22, Numeros = new List<int>{1,2,6,24}, Respuesta = 120, Dificultad="Medio" },
            new Secuencia { Id = 23, Numeros = new List<int>{5,10,20,40,80}, Respuesta = 160, Dificultad="Medio" },
            new Secuencia { Id = 24, Numeros = new List<int>{2,4,8,14,22}, Respuesta = 32, Dificultad="Medio" },
            new Secuencia { Id = 25, Numeros = new List<int>{1,3,6,10,15}, Respuesta = 21, Dificultad="Medio" }
        };

        private static readonly List<Secuencia> SecuenciasDificil = new()
        {
            new Secuencia { Id = 21, Numeros = new List<int>{1,2,6,24,120}, Respuesta = 720, Dificultad="Dificil" },
            new Secuencia { Id = 22, Numeros = new List<int>{1,1,2,6,24,120}, Respuesta = 720, Dificultad="Dificil" },
            new Secuencia { Id = 23, Numeros = new List<int>{2,3,5,8,13,21}, Respuesta = 34, Dificultad="Dificil" },
            new Secuencia { Id = 24, Numeros = new List<int>{1,4,9,16,25,36}, Respuesta = 49, Dificultad="Dificil" },
            new Secuencia { Id = 25, Numeros = new List<int>{1,2,4,8,16,32,64}, Respuesta = 128, Dificultad="Dificil" },
            new Secuencia { Id = 26, Numeros = new List<int>{3,5,9,17,33}, Respuesta = 65, Dificultad="Dificil" },
            new Secuencia { Id = 27, Numeros = new List<int>{2,6,12,20,30,42}, Respuesta = 56, Dificultad="Dificil" },
            new Secuencia { Id = 28, Numeros = new List<int>{1,3,7,15,31}, Respuesta = 63, Dificultad="Dificil" },
            new Secuencia { Id = 29, Numeros = new List<int>{2,3,5,10,17,26}, Respuesta = 37, Dificultad="Dificil" },
            new Secuencia { Id = 30, Numeros = new List<int>{1,2,3,5,8,13,21}, Respuesta = 34, Dificultad="Dificil" }
        };

        private static readonly Random rand = new Random();

        // GET: Racha actual del usuario
        [HttpGet("racha/{usuarioId}/{modo}")]
        public async Task<IActionResult> ObtenerRacha(int usuarioId, int modo, [FromServices] AppDbContext db)
        {
            var racha = await db.Rachas.FirstOrDefaultAsync(r => r.UsuarioId == usuarioId && r.Modo == (ModoJuego)modo);
            return Ok(new { actual = racha?.Actual ?? 0, maxima = racha?.Maxima ?? 0 });
        }

        // GET: Obtener secuencia aleatoria según racha
        [HttpGet("obtener")]
        public async Task<IActionResult> ObtenerSecuencia([FromQuery] int usuarioId, [FromQuery] int modo, [FromServices] AppDbContext db)
        {
            var racha = await db.Rachas.FirstOrDefaultAsync(r => r.UsuarioId == usuarioId && r.Modo == (ModoJuego)modo);
            int rachaActual = racha?.Actual ?? 0;

            string dificultad = rachaActual < 7 ? "Facil" :
                                rachaActual < 14 ? "Medio" : "Dificil";

            List<Secuencia> lista = dificultad switch
            {
                "Facil" => SecuenciasFacil,
                "Medio" => SecuenciasMedio,
                "Dificil" => SecuenciasDificil,
                _ => new List<Secuencia>()
            };

            if (lista == null || lista.Count == 0) return NotFound(new { mensaje = "No hay secuencias disponibles" });

            var secuencia = lista[rand.Next(lista.Count)];

            // Devolver secuencia al cliente (sin exponer respuesta correcta)
            return Ok(new
            {
                numeros = secuencia.Numeros,
                dificultad = secuencia.Dificultad,
                secuenciaId = secuencia.Id
            });
        }

        // POST: Evaluar respuesta del usuario
        [HttpPost("responder")]
        public async Task<IActionResult> Responder([FromBody] RespuestaUsuarioDto dto, [FromServices] AppDbContext db)
        {
            var secuencia = SecuenciasFacil.Concat(SecuenciasMedio).Concat(SecuenciasDificil)
                                           .FirstOrDefault(s => s.Id == dto.SecuenciaId);
            if (secuencia == null) return NotFound(new { mensaje = "Secuencia no encontrada" });

            bool esCorrecta = dto.RespuestaUsuario == secuencia.Respuesta;

            if (dto.UsuarioId <= 0)
                return BadRequest(new { mensaje = "UserId es requerido" });

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

            try { await db.SaveChangesAsync(); }
            catch (DbUpdateException ex)
            {
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
