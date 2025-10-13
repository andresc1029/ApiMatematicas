namespace ApiMatematicas.Strategy.RecupearContraseñaStrategy
{
    public interface IRecuperarContrasena
    {
        Task EnviarCodigoRecuperacionAsync(string correo);
        Task<bool> ValidarCodigoAsync(string correo, string token);

        Task<bool> RestablecerContraseñaAsync(string correo,string token, string codigo);
    }
}
