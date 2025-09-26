namespace ApiMatematicas.Models
{
    public class RespuestaUsuarioDto
    {
        public int SecuenciaId { get; set; }
        public int RespuestaUsuario { get; set; }
        public int UsuarioId { get; set; }

        // Enum que se puede enviar como número: 0,1,2
        public ModoJuego Modo { get; set; }
    }
}
