using articulomodelo.Backend.Modelo;

namespace articulomodelo.Backend.Servicios
{
    /// <summary>
    /// Interfaz específica para el repositorio de <see cref="Articulo"/>.
    /// Extiende las operaciones CRUD genéricas y añade consultas útiles para artículos.
    /// </summary>
    public interface IArticuloRepository : IGenericRepository<Articulo>
    {
   
    }
}