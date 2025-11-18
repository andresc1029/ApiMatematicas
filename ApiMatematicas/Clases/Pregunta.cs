using System.Text.Json.Serialization;

namespace ApiMatematicas.Clases
{
    public class Pregunta
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("titulo")]
        public string Titulo { get; set; } = "";

        [JsonPropertyName("imagen")]
        public string Imagen { get; set; } = "";

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = "";

        [JsonPropertyName("opciones")]
        public List<string> Opciones { get; set; }

        [JsonPropertyName("respuesta")] 
        public string RespuestaCorrecta { get; set; } = "";
    }
}
