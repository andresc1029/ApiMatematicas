namespace ApiMatematicas.DTO
{
    public class TopUsuarioDTO
    {
        public int UsuarioId {get; set; } 
        public string NombreUsuario { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public int RachaMaxima { get; set; }
        public int RachaActual { get; set; }
        public int Modo { get; set; }
    }
}
