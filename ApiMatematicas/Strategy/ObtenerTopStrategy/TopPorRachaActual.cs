using ApiMatematicas.Models;
using System.Linq;

namespace ApiMatematicas.Strategy.ObtenerTopStrategy
{
    public class TopPorRachaActual : ITopStrategy
    {
        public IQueryable<SistemaRacha> ObtenerTop(IQueryable<SistemaRacha> rachas)
        {
            return rachas.OrderByDescending(r => r.actual).Take(15);
        }
    }
}
