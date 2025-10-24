using System.ComponentModel.DataAnnotations;

namespace ApiMatematicas.DTO
{
    public class TopCompletoDTO
    {

        public List<TopUsuarioDTO>? Modulo0_Historico { get; set; }
        public List<TopUsuarioDTO>? Modulo0_Actual { get; set; }

        public List<TopUsuarioDTO>? Modulo1_Historico { get; set; }
        public List<TopUsuarioDTO>? Modulo1_Actual { get; set; }

        public List<TopUsuarioDTO>? Modulo2_Historico { get; set; }
        public List<TopUsuarioDTO>? Modulo2_Actual { get; set; }
    }
}
