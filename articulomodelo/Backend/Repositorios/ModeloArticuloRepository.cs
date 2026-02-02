using articulomodelo.Backend.Modelo;
using Microsoft.Extensions.Logging;

namespace articulomodelo.Backend.Servicios
{
    public class ModeloArticuloRepository : GenericRepository<Modeloarticulo>, IModeloArticuloRepository
    {
        public ModeloArticuloRepository(DiinventarioexamenContext context, ILogger<GenericRepository<Modeloarticulo>> logger)
            : base(context, logger)
        {
        }
    }
}
