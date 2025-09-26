using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiMatematicas.Models
{
    public class SistemaRacha
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        [JsonIgnore]
        public Usuario? Usuario { get; set; }

        public ModoJuego Modo { get; set; } = ModoJuego.SecuenciasNumericas;

        public int Actual { get; set; } = 0;
        public int Maxima { get; set; } = 0;

        public DateTime FechaUltimaActualizacion { get; set; } = DateTime.UtcNow;
        public DateTime? InicioRachaActual { get; set; } = null;
    }
}
