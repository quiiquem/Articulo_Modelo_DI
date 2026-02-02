using articulomodelo.Backend.Modelo;
using Microsoft.Extensions.Logging;

namespace articulomodelo.Backend.Servicios
{
    public class TipoArticuloRepository : GenericRepository<Tipoarticulo>, ITipoArticuloRepository
    {
        public TipoArticuloRepository(DiinventarioexamenContext context, ILogger<GenericRepository<Tipoarticulo>> logger) : base(context, logger)
        {
        }
    }
}
