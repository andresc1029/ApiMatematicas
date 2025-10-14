using ApiMatematicas.Clases;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace ApiMatematicas.Strategy.ProblemasCondicionesStrategy
{
    public class CondicionesStrategy
    {
        private readonly Dictionary<string, List<ProblemaCondicional>> _problemas;
        private readonly Random rand = new();

        public CondicionesStrategy()
        {
            string ruta = Path.Combine(AppContext.BaseDirectory, "JSON", "ProblemasCondicionesJson", "problemas_condiciones.json");


            if (!File.Exists(ruta))
                throw new FileNotFoundException($"No se encontró el archivo de problemas: {ruta}");

            string json = File.ReadAllText(ruta);
            _problemas = JsonSerializer.Deserialize<Dictionary<string, List<ProblemaCondicional>>>(json)
                ?? new Dictionary<string, List<ProblemaCondicional>>();
        }

        public ProblemaCondicional GenerarProblema(string dificultad)
        {
            if (!_problemas.ContainsKey(dificultad))
                dificultad = "Facil";

            var lista = _problemas[dificultad];
            return lista[rand.Next(lista.Count)];
        }

        public ProblemaCondicional? ObtenerProblemaPorId(int id)
        {
            return _problemas.Values.SelectMany(x => x)
                                    .FirstOrDefault(p => p.Id == id);
        }

        public bool EvaluarRespuesta(int idCondicion, string respuestaUsuario)
        {
            var problema = _problemas.Values.SelectMany(x => x)
                                            .FirstOrDefault(p => p.Id == idCondicion);

            if (problema == null || string.IsNullOrWhiteSpace(respuestaUsuario))
                return false;

            string esperada = Normalizar(problema.RespuestaCorrecta);
            string usuario = Normalizar(respuestaUsuario);

            return esperada == usuario;
        }

        private string Normalizar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "";

            var formD = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in formD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().ToLowerInvariant().Trim();
        }
    }
}
