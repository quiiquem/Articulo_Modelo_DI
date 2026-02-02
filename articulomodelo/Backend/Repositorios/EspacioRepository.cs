using articulomodelo.Backend.Modelo;
using Microsoft.Extensions.Logging;

namespace articulomodelo.Backend.Servicios
{
    public class EspacioRepository : GenericRepository<Espacio>, IEspacioRepository
    {
        public EspacioRepository(DiinventarioexamenContext context, ILogger<GenericRepository<Espacio>> logger)
            : base(context, logger)
        {
        }
    }
}
