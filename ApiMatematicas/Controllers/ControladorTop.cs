using ApiMatematicas.Data;
using ApiMatematicas.DTO;
using ApiMatematicas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMatematicas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TopController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("completo")]
        public async Task<ActionResult<TopCompletoDTO>> GetTopCompleto()
        {
            var resultado = new TopCompletoDTO();

            for (int modo = 0; modo <= 2; modo++)
            {
                // Top histórico
                var topMax = await _context.Rachas
                    .Where(r => (int)r.modo == modo)
                    .OrderByDescending(r => r.maxima)
                    .Take(15)
                    .ToListAsync();

                // Top actual 
                var topActual = await _context.Rachas
                    .Where(r => (int)r.modo == modo)
                    .OrderByDescending(r => r.actual)
                    .Take(15)
                    .ToListAsync();

                var usuarioIdsMax = topMax.Select(r => r.usuarioId).ToList();
                var usuarioIdsActual = topActual.Select(r => r.usuarioId).ToList();

                var usuariosMax = await _context.Usuarios
                    .Where(u => usuarioIdsMax.Contains(u.id))
                    .ToListAsync();

                var usuariosActual = await _context.Usuarios
                    .Where(u => usuarioIdsActual.Contains(u.id))
                    .ToListAsync();

                var topMaxDTO = topMax.Join(usuariosMax, r => r.usuarioId, u => u.id, (r, u) => new TopUsuarioDTO
                {
                    UsuarioId = u.id,
                    NombreUsuario = u.nombreUsuario,
                    RachaMaxima = r.maxima,
                    RachaActual = r.actual,
                    Modo = modo
                }).ToList();

                var topActualDTO = topActual.Join(usuariosActual, r => r.usuarioId, u => u.id, (r, u) => new TopUsuarioDTO
                {
                    UsuarioId = u.id,
                    NombreUsuario = u.nombreUsuario,
                    RachaMaxima = r.maxima,
                    RachaActual = r.actual,
                    Modo = modo
                }).ToList();

                switch (modo)
                {
                    case 0:
                        resultado.Modulo0_Historico = topMaxDTO;
                        resultado.Modulo0_Actual = topActualDTO;
                        break;
                    case 1:
                        resultado.Modulo1_Historico = topMaxDTO;
                        resultado.Modulo1_Actual = topActualDTO;
                        break;
                    case 2:
                        resultado.Modulo2_Historico = topMaxDTO;
                        resultado.Modulo2_Actual = topActualDTO;
                        break;
                }
            }

            return resultado;
        }

        
        //// GET api/top/{modo}
        //[HttpGet("{modo}")]
        //public async Task<ActionResult<List<TopUsuarioDTO>>> GetTop(int modo)
        //{
        //    // Validar enum
        //    if (!Enum.IsDefined(typeof(ModoJuego), modo))
        //        return BadRequest("Modo inválido");

        //    ModoJuego modoJuego = (ModoJuego)modo;

        //    // Traer las 15 rachas máximas por ese modo
        //    var rachas = await _context.Rachas
        //        .Where(r => r.Modo == modoJuego)
        //        .OrderByDescending(r => r.Maxima)
        //        .Take(15)
        //        .ToListAsync();

        //    // IDs de los usuarios
        //    var usuarioIds = rachas.Select(r => r.UsuarioId).ToList();

        //    // Traer los usuarios
        //    var usuarios = await _context.Usuarios
        //        .Where(u => usuarioIds.Contains(u.id))
        //        .ToListAsync();

        //    // Hacer join en memoria
        //    var topUsuariosDTO = rachas
        //        .Join(usuarios,
        //              r => r.UsuarioId,
        //              u => u.id,
        //              (r, u) => new TopUsuarioDTO
        //              {
        //                  UsuarioId = u.id,
        //                  NombreUsuario = u.nombreUsuario,
        //                  RachaMaxima = r.Maxima,
        //                  RachaActual = r.Actual,
        //                  Modo = (int)r.Modo
        //              })
        //        .ToList();

        //    return topUsuariosDTO;

        //}
    }

}
