using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiMatematicas.Models
{
    public class ReinicioContrasenaToken
    {
        public int Id { get; set; }

        // Relación con el usuario
        public int UserId { get; set; }

        [JsonIgnore]
        public Usuario User { get; set; }

        // Token y expiración
        public required string Token { get; set; }

        public DateTime Expiration { get; set; } = DateTime.UtcNow;
    }
}
