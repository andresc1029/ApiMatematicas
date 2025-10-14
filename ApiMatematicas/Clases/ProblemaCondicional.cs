namespace ApiMatematicas.Clases
{
    public class ProblemaCondicional
    {
        public int Id { get; set; }
        public string Enunciado { get; set; } = string.Empty;
        public string? OpcionA { get; set; }
        public string? OpcionB { get; set; }
        public string? OpcionC { get; set; }
        public string? OpcionD { get; set; }
        public string RespuestaCorrecta { get; set; } = string.Empty;
        public string Dificultad { get; set; } = "Facil";
    }
}
