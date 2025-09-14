using Microsoft.AspNetCore.Mvc;

namespace ApiMatematicas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControladorMatematicas : ControllerBase
    {
        private static Random rand = new Random();


        [HttpGet("random")]
        public IActionResult GetRandom()
        {
            int a = rand.Next(1, 10);
            int b = rand.Next(1, 10);
            return Ok(new { numero1 = a,numero2 = b });
        }

        [HttpPost ("pregunta")]
        public IActionResult PostPregunta([FromBody] AnswerRequest request)
        {
            bool correcto =  request.respuestaUsuario == request.numero1 * request.numero2;
            return Ok(new { correcto });
        }
    }

    public class AnswerRequest
    {
        public int numero1 { get; set; }
        public int numero2 { get; set; }
        public int respuestaUsuario { get; set; }
    }

}
