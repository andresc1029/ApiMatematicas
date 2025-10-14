using ApiMatematicas.Models;
using ApiMatematicas.Strategy.InicioSesionStrateg;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static ApiMatematicas.Strategy.InicioSesionStrateg.IRecuperarTokenInicioStrategy;

namespace ApiMatematicas.Strategy.InicioSesionStrategy
{
    public class JwtTokenStrategy : IRecuperarTokenInicioStrategy
    {
        private readonly IConfiguration _config;

        public JwtTokenStrategy(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Usuario usuario)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var duration = double.Parse(jwtSettings["DurationInMinutes"]);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.nombreUsuario),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userId", usuario.id.ToString()),
            new Claim("Correo", usuario.Correo ?? "")
        };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(duration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void SetCookie(HttpResponse response, string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, 
                Secure = true,    
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.Now.AddMinutes(60) 
            };
            response.Cookies.Append("jwt", token, cookieOptions);
        }
    }
}
