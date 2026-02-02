using articulomodelo.Backend.Modelo;
using Microsoft.Extensions.Logging;

namespace articulomodelo.Backend.Servicios
{
    public class TipoUsuarioRepository : GenericRepository<Tipousuario>
    {
        public TipoUsuarioRepository(DiinventarioexamenContext context, ILogger<GenericRepository<Tipousuario>> logger) : base(context, logger)
        {
        }
    }
}
