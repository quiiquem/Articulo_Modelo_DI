using articulomodelo.Backend.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace articulomodelo.Backend.Servicios
{
    /// <summary>
    /// Repositorio específico para la entidad <see cref="Articulo"/>.
    /// Implementa consultas comunes y utilidades relacionadas con artículos.
    /// </summary>
    public class ArticuloRepository : GenericRepository<Articulo>, IArticuloRepository
    {
        public ArticuloRepository(DiinventarioexamenContext context, ILogger<GenericRepository<Articulo>> logger)
            : base(context, logger)
        {
        }

        //Guardar el articulo 
        public async Task<int> ObtenerMaximoId()
        {
            var maxId = await _context.Articulos
                .MaxAsync(a => (int?)a.Idarticulo) ?? 0;
            return maxId;
        }

        public async Task<List<Articulo>> GetAllWithRelationsAsync()
        {
            return await _context.Articulos
                .Include(a => a.ModeloNavigation)
                .Include(a => a.tipoEspacioSeleccionado)
                .Include(a => a.DepartamentoNavigation)
                .Include(a => a.UsuarioaltaNavigation)
                .Include(a => a.UsuariobajaNavigation)
                .Include(a => a.DentrodeNavigation)
                .ToListAsync();
        }
    }
}