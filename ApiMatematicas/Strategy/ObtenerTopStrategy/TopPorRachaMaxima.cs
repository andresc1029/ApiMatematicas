using ApiMatematicas.Models;
using System.Linq;

namespace ApiMatematicas.Strategy.ObtenerTopStrategy
{
    public class TopPorRachaMaxima : ITopStrategy
    {
        public IQueryable<SistemaRacha> ObtenerTop(IQueryable<SistemaRacha> rachas)
        {
            return rachas.OrderByDescending(r => r.maxima).Take(15);
        }
    }
}
