using Microsoft.VisualBasic;

namespace ApiMatematicas.Models
{
    public class Usuario
    {
        public int id {get; set; }
        public string nombreUsuario { get; set; } = string.Empty;
        public string contrasena { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Correo { get; set; } = string.Empty;
        public DateTime fechaRegistro { get; set; } = DateTime.Now;
        public string rol { get; set; } = "Usuario";
        public bool activo { get; set; } = true;

        public ICollection<SistemaRacha> Rachas { get; set; } = new List<SistemaRacha>();
    }
}