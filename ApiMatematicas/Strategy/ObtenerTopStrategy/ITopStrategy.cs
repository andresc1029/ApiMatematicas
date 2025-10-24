using ApiMatematicas.Models;
using System.Linq;

namespace ApiMatematicas.Strategy.ObtenerTopStrategy
{
    public interface ITopStrategy
    {
        IQueryable<SistemaRacha> ObtenerTop(IQueryable<SistemaRacha> rachas);
    }
}

