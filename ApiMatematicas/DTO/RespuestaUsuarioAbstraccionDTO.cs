using ApiMatematicas.Models;

namespace ApiMatematicas.DTO
{
    public class RespuestaUsuarioAbstraccionDTO
    {
        public int UsuarioId { get; set; }
        public int PreguntaId { get; set; }
        public string RespuestaUsuario { get; set; } = string.Empty;
        public ModoJuego Modo { get; set; }
    }
}
