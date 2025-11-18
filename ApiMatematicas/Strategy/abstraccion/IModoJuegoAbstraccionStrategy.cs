using ApiMatematicas.Clases;

namespace ApiMatematicas.Strategy.abstraccion
{
    public interface IModoJuegoAbstraccionStrategy
    {     
        Pregunta GenerarPregunta(int usuarioId);

        bool EvaluarRespuesta(int usuarioId, int preguntaId, string respuestaUsuario);

    }
}
