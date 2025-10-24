using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiMatematicas.Models
{
    public class SistemaRacha
    {
        public int id { get; set; }
        public int usuarioId { get; set; }

        [JsonIgnore]
        public Usuario? usuario { get; set; }

        public ModoJuego modo { get; set; } = ModoJuego.SecuenciasNumericas;

        public int actual { get; set; } = 0;
        public int maxima { get; set; } = 0;

        public DateTime fechaUltimaActualizacion { get; set; } = DateTime.UtcNow;
        public DateTime? inicioRachaActual { get; set; } = null;
    }
}
