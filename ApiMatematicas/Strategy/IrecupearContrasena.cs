namespace ApiMatematicas.Strategy
{
    public interface IRecuperarContrasena
    {
        Task EnviarCodigoRecuperacionAsync(string correo);
        Task<bool> RestablecerContraseñaAsync(string correo,string token, string codigo);
    }
}
