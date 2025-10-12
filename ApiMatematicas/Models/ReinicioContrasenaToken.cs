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
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
