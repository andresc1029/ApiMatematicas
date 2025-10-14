namespace ApiMatematicas.Strategy.ProblemasCondicionesStrategy
{
    public interface InterfazCondiciones
    {
        string GenerarProblema(string dificultad);
        bool EvaluarRespuesta(int problemaId, string respuestaUsuario);
    }
}
