using ApiMatematicas.Models;

namespace ApiMatematicas.Strategy.InicioSesionStrateg
{
    public interface IRecuperarTokenInicioStrategy
    {
            string GenerateToken(Usuario usuario);
            void SetCookie(HttpResponse response, string token);
    }
}
