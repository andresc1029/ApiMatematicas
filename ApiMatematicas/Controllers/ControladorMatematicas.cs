using Microsoft.AspNetCore.Mvc;

namespace ApiMatematicas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorMatematicas : ControllerBase
    {
        private static Random rand = new Random();
        private static int contadorDeRacha = 0;
        private static DateTime tiempoInicioPregunta = DateTime.MinValue;
        private static bool? modoAnterior = null; 

        [HttpGet("random")]
        public IActionResult GetRandom()
        {
            int a = rand.Next(1, 10);
            int b = rand.Next(1, 10);
            return Ok(new { numero1 = a, numero2 = b });
        }

        [HttpPost("pregunta")]
        public IActionResult PostPregunta([FromBody] AnswerRequest request)
        {
            bool correcto = request.respuestaUsuario == request.numero1 * request.numero2;

            // reiniciar la racha
            if (modoAnterior != null && modoAnterior != request.ModoConTiempo)
            {
                contadorDeRacha = 0;
                tiempoInicioPregunta = DateTime.MinValue;
            }
            modoAnterior = request.ModoConTiempo;

            if (request.ModoConTiempo)
            {
                if (tiempoInicioPregunta == DateTime.MinValue)
                {
                    tiempoInicioPregunta = DateTime.UtcNow;
                }

                var tiempoTranscurrido = DateTime.UtcNow - tiempoInicioPregunta;

                int limiteTiempo = contadorDeRacha >= 15 ? 5 :  (contadorDeRacha >= 10 ? 10 : 15);
                if (tiempoTranscurrido.TotalSeconds > limiteTiempo)
                {
                    contadorDeRacha = 0;
                    tiempoInicioPregunta = DateTime.MinValue;
                    return Ok(new { correcto = false, racha = contadorDeRacha, tiempoAgotado = true, tiempoInicio = limiteTiempo });
                }

                if (correcto)
                {
                    contadorDeRacha++;
                    tiempoInicioPregunta = DateTime.UtcNow;
                }
                else
                {
                    contadorDeRacha = 0;
                }
                int tiempoRestante = Math.Max(0, limiteTiempo - (int)tiempoTranscurrido.TotalSeconds);

                return Ok(new { correcto, racha = contadorDeRacha, tiempoUsuario = tiempoRestante, tiempoInicio = limiteTiempo });
            }
            else
            {
                if (correcto)
                {
                    contadorDeRacha++;
                }
                else
                {
                    contadorDeRacha = 0;
                }
                return Ok(new { correcto, racha = contadorDeRacha });
            }
        }
    }

    public class AnswerRequest
    {
        public int numero1 { get; set; }
        public int numero2 { get; set; }
        public int respuestaUsuario { get; set; }
        public bool ModoConTiempo { get; set; }
    }
}
