using articulomodelo.Backend.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace articulomodelo.Backend.Servicios
{
    public class EspacioRepository : GenericRepository<Espacio>, IEspacioRepository
    {
        public EspacioRepository(DiinventarioexamenContext context, ILogger<GenericRepository<Espacio>> logger)
            : base(context, logger)
        {
        }

        public async Task<List<Espacio>> GetAllConArticulosAsync()
        {
            return await _dbSet
                .Include(e => e.Articulos)
                .ToListAsync();
        }
    }
}