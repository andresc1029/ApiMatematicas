using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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


        [JsonIgnore]
        public ICollection<SistemaRacha> Rachas { get; set; } = new List<SistemaRacha>();

        [JsonIgnore]
        public ICollection<ReinicioContrasenaToken> ReinicioContraseñas { get; set; } = new List<ReinicioContrasenaToken>();

    }
}