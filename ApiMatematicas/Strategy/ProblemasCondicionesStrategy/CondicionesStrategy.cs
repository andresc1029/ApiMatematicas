using ApiMatematicas.Clases;
using System.Globalization;
using System.Text;
namespace ApiMatematicas.Strategy.ProblemasCondicionesStrategy
{
    public class CondicionesStrategy
    {
        private static readonly Random rand = new();

        private static readonly List<ProblemaCondicional> problemasFacil = new()
        {
          new ProblemaCondicional
            {
                Id = 1001,
                Enunciado = "Si X = 10, Y = 15, y Z = 5, ¿cuál es menor?",
                OpcionA = "X",
                OpcionB = "Y",
                OpcionC = "Z",
                RespuestaCorrecta = "Z",
                Dificultad = "Facil"
            },
            new ProblemaCondicional
            {
                Id = 1002,
                Enunciado = "Si A > B y B > C, entonces ¿A es mayor que C?",
                OpcionA = "Sí",
                OpcionB = "No",
                OpcionC = "Depende",
                RespuestaCorrecta = "Sí",
                Dificultad = "Facil"
            },
            new ProblemaCondicional
            {
                Id = 1003,
                Enunciado = "Si hoy es lunes, ¿qué día será dentro de 2 días?",
                OpcionA = "Martes",
                OpcionB = "Miércoles",
                OpcionC = "Jueves",
                RespuestaCorrecta = "Miércoles",
                Dificultad = "Facil"
            }
        };

        private static readonly List<ProblemaCondicional> problemasMedio = new()
        {
            new ProblemaCondicional
            {
                Id = 2001,
                Enunciado = "Si A > B, B < C y C = D, ¿cuál es menor?",
                OpcionA = "A",
                OpcionB = "B",
                OpcionC = "C",
                OpcionD = "D",
                RespuestaCorrecta = "B",
                Dificultad = "Medio"
            },
            new ProblemaCondicional
            {
                Id = 2002,
                Enunciado = "Si Pedro es mayor que Juan, y Juan mayor que Ana, ¿quién es más joven?",
                OpcionA = "Pedro",
                OpcionB = "Juan",
                OpcionC = "Ana",
                RespuestaCorrecta = "Ana",
                Dificultad = "Medio"
            },
            new ProblemaCondicional
            {
                Id = 2003,
                Enunciado = "Si X = 2Y y Y = 3Z, ¿qué valor tiene X en función de Z?",
                OpcionA = "X = 5Z",
                OpcionB = "X = 6Z",
                OpcionC = "X = Z/6",
                RespuestaCorrecta = "X = 6Z",
                Dificultad = "Medio"
            },
            new ProblemaCondicional
            {
                Id = 2004,
                Enunciado = "Si A + B = 10 y A = 6, ¿cuánto vale B?",
                OpcionA = "4",
                OpcionB = "6",
                OpcionC = "10",
                RespuestaCorrecta = "4",
                Dificultad = "Medio"
            }
        };

        private static readonly List<ProblemaCondicional> ProblemasDificil = new()
        {
            new ProblemaCondicional
            {
                Id = 3001,
                Enunciado = "Si A > B y B > C pero C > D, ¿cuál podría ser el orden correcto de menor a mayor?",
                OpcionA = "D, C, B, A",
                OpcionB = "C, D, B, A",
                OpcionC = "B, C, D, A",
                Dificultad = "Dificil"
            },
            new ProblemaCondicional
            {
                Id = 3002,
                Enunciado = "Si X > Y, Y > Z y Z = 0, y además X + Y = 10, ¿cuál podría ser un valor posible de X?",
                OpcionA = "6",
                OpcionB = "4",
                OpcionC = "2",
                Dificultad = "Dificil"
            },
            new ProblemaCondicional
            {
                Id = 3003,
                Enunciado = "Si A + B = 12, B + C = 14 y A + C = 16, ¿cuánto vale A?",
                OpcionA = "7",
                OpcionB = "8",
                OpcionC = "9",
                Dificultad = "Dificil"
            },
            new ProblemaCondicional
            {
                Id = 3004,
                Enunciado = "Si cada afirmación falsa suma -1 y cada verdadera suma +2, y obtienes 3 puntos tras 3 afirmaciones, ¿cuántas fueron verdaderas?",
                OpcionA = "1",
                OpcionB = "2",
                OpcionC = "3",
                Dificultad = "Dificil"
            }
        };

        public ProblemaCondicional GenerarProblema(string dificultad)
        {
            List<ProblemaCondicional> lista = dificultad switch
            {
                "Facil" => problemasFacil,
                "Medio" => problemasMedio,
                "Dificil" => ProblemasDificil,
                _ => problemasFacil
            };

            return lista[rand.Next(lista.Count)];
        }

        public ProblemaCondicional? ObtenerProblemaPorId(int id)
        {
            return problemasFacil.Concat(problemasMedio).Concat(ProblemasDificil)
                                  .FirstOrDefault(p => p.Id == id);
        }

        public bool EvaluarRespuesta(int idCondicion, string respuestaUsuario)
        {
            var todosLosProblemas = problemasFacil.Concat(problemasMedio).Concat(ProblemasDificil);
            var problema = todosLosProblemas.FirstOrDefault(p => p.Id == idCondicion);

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

            // Normaliza tildes (ej: "Sí" → "si") y pone todo en minúsculas
            var formD = texto.Normalize(NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();

            foreach (var c in formD)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString()
                     .ToLowerInvariant()
                     .Trim();
        }
    }
}
