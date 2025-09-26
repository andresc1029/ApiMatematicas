namespace ApiMatematicas.Clases
{
    public class Secuencia
    {
        public int Id { get; set; }
        public List<int> Numeros { get; set; } = new();
        public int Respuesta { get; set; }
        public string Dificultad { get; set; } = "Facil"; // Facil, Medio, Dificil
    }
}
