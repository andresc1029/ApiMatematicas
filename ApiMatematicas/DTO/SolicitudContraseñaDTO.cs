namespace ApiMatematicas.DTO
{
    public class SolicitudContraseñaDTO
    {

        public string CorreoUsuario { get; set; }

    }

    public class CambioContraseñaDTO
    {
        public string CorreoUsuario { get; set; }
        public string NuevaContraseña { get; set; }
        public string Token { get; set; }
    }
}
