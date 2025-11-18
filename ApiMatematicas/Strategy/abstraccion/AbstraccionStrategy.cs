using ApiMatematicas.Clases;
using ApiMatematicas.Strategy.abstraccion;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace ApiMatematicas.Strategy.AbstaccionStrategy
{
    public class AbstraccionStrategy : IModoJuegoAbstraccionStrategy
    {
        private readonly List<Pregunta> _preguntas;
        private readonly Random _rand = new();

        public AbstraccionStrategy()
        {
            string ruta = Path.Combine(AppContext.BaseDirectory, "JSON", "Abstracto", "Abstraccion.json");

            if (!File.Exists(ruta))
                throw new FileNotFoundException($"No se encontró el archivo de preguntas: {ruta}");

            string json = File.ReadAllText(ruta);

            _preguntas = JsonSerializer.Deserialize<List<Pregunta>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Pregunta>();
        }



        public Pregunta GenerarPregunta(int usuarioId)
        {
            if (_preguntas.Count == 0) return null!;
            return _preguntas[_rand.Next(_preguntas.Count)];
        }

        public bool EvaluarRespuesta(int usuarioId, int preguntaId, string respuestaUsuario)
        {
            var pregunta = ObtenerPreguntaPorId(preguntaId);
            if (pregunta == null || string.IsNullOrWhiteSpace(respuestaUsuario))
                return false;

            return Normalizar(pregunta.RespuestaCorrecta) == Normalizar(respuestaUsuario);
        }


        public Pregunta? ObtenerPreguntaPorId(int id)
        {
            return _preguntas.FirstOrDefault(p => p.Id == id);
        }

        private string Normalizar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "";

            var formD = texto.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in formD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().ToLowerInvariant().Trim();
        }
    }
}
