namespace ApiMatematicas.Models
{
    public class RespuestaUsuarioDto
    {
        public int SecuenciaId { get; set; }
        public int RespuestaUsuario { get; set; }
        public int UsuarioId { get; set; }

        // Acepta 0,1,2
        public ModoJuego Modo { get; set; }
    }
}
