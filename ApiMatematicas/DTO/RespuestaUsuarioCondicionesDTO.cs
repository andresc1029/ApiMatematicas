using ApiMatematicas.Models;

namespace ApiMatematicas.DTO
{
    public class RespuestaUsuarioCondicionesDTO
    {
        public int ProblemaId { get; set; }
        public string RespuestaUsuario { get; set; } = string.Empty;
        public int UsuarioId { get; set; }

        public ModoJuego Modo { get; set; }
    }
}
