using articulomodelo.Backend.Modelo;
using articulomodelo.Backend.Servicios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace articulomodelo.Backend.Servicios
{
    public class DepartamentoRepository : GenericRepository<Departamento>, IDepartamentoRepository
    {
        public DepartamentoRepository(DiinventarioexamenContext context, ILogger<GenericRepository<Departamento>> logger)
             : base(context, logger)
        {
        }


    }
}