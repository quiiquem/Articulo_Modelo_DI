using articulomodelo.Backend.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace articulomodelo.Backend.Servicios
{
    /// <summary>
    /// Repositorio específico para la entidad <see cref="Articulo"/>.
    /// Implementa consultas comunes y utilidades relacionadas con artículos.
    /// </summary>
    public class GrupoRepository : GenericRepository<Grupo>
    {
        public GrupoRepository(DiinventarioexamenContext context, ILogger<GenericRepository<Grupo>> logger)
            : base(context, logger)
        {
        }
    }
}