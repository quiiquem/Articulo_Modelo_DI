using articulomodelo.Backend.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace articulomodelo.Backend.Servicios
{
    /// <summary>
    /// Repositorio específico para la entidad <see cref="Rol"/>.
    /// Implementa consultas comunes y utilidades relacionadas con artículos.
    /// </summary>
    public class RolRepository : GenericRepository<Rol>
    {
        public RolRepository(DiinventarioexamenContext context, ILogger<GenericRepository<Rol>> logger)
            : base(context, logger)
        {
        }
    }
}