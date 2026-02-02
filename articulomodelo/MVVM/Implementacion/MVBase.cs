using articulomodelo.Backend.Servicios;
using articulomodelo.Frontend.Mensajes;
using MaterialDesignThemes.Wpf;
using ProyectoDI_Trimestre1.Frontend.Mensajes;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Validation = System.Windows.Controls.Validation;

namespace articulomodelo.MVVM.Implementacion
{ 
    public class MVBase : ValidatableViewModel
    {
        /// <summary>
        /// Botón del formulario que queremos que se active/desactive en función
        /// de si hay errores en la validación de los campos
        /// </summary>
        public bool HasErrors
        {
            get => _hasErrors;
            set => SetProperty(ref _hasErrors, value);
        }

        private bool _hasErrors;
        
        /// <summary>
        /// Variable que llev la cuenta de los errores que hay en el formulario
        /// </summary>
        private int errorCount = 0;
        public SnackbarMessageQueue SnackbarMessageQueue { get; } = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

        // Añade métodos que nos ayudan en la validación -------------------------------------------------------------------------------
        /// <summary>
        /// Método que nos permite saber si hay errores en algún formulario
        /// </summary>
        /// <param name="obj">Ventana o panel que contiene los controles del formulario que queremos comprobar</param>
        /// <returns>n caso de que haya errores devolverá el valor de falso y en caso de que no haya devolverá verdadero</returns>
        public bool IsValid(DependencyObject obj)
        {
            // The dependency object is valid if it has no errors and all
            // of its children (that are dependency objects) are error-free.
            return !Validation.GetHasError(obj) &&
            LogicalTreeHelper.GetChildren(obj)
            .OfType<DependencyObject>()
            .All(IsValid);
        }

        /// <summary>
        /// Manejador de eventos para la validación de errores en los controles del formulario.
        /// Si se añade un error, incrementa el contador de errores; si se elimina, lo decrementa.
        /// Luego utiliza la propiedad HasErrors para activar o desactivar el botón de guardar.
        /// </summary>
        /// <param name="sender">Control que produce el error de validación</param>
        /// <param name="e">Parámetros del error</param>
        public void OnErrorEvent(object sender, RoutedEventArgs e)
        {
            var validationEventArgs = e as ValidationErrorEventArgs;
            if (validationEventArgs == null)
                throw new Exception("Argumentos inesperados");
            switch (validationEventArgs.Action)
            {
                case ValidationErrorEventAction.Added:
                    {
                        errorCount++; break;
                    }
                case ValidationErrorEventAction.Removed:
                    {
                        errorCount--; break;
                    }
                default:
                    {
                        throw new Exception("Acción desconocida");
                    }
            }
            UpdateHasErrors();
        }

        /// <summary>
        /// Actualiza HasErrors en función del contador interno.
        /// </summary>
        private void UpdateHasErrors()
        {
            HasErrors = errorCount == 0;
        }

        // Métodos CRUD genéricos asíncronos con manejo de excepciones
        /// <summary>
        /// Obtiene la lista asociada a una tabla de la base de datos
        /// </summary>
        /// <typeparam name="T">Entidad que representa el objeto asociado a la tabla</typeparam>
        /// <param name="repo">Repositorio utilizado para el acceso a datos</param>
        /// <returns>La lista con los objetos de la tabla o 
        /// bien una lista vacía en caso de que haya un problema. 
        /// Esto evita problemas en la interfaz de usuario</returns>
        protected async Task<List<T>> GetAllAsync<T>(IGenericRepository<T> repo) where T : class
        {
            List<T> resultado;
            try
            {
                resultado = await repo.GetAllAsync();
            }
            catch (DataAccessException dae)
            {
                resultado = new List<T>();
                // Mensaje claro para el usuario
                MensajeError.Mostrar("ACCESO A LOS DATOS","Error de datos: No se puede acceder a los datos");
            }
            return resultado.ToList();
        }
        /// <summary>
        /// Obtiene un elemento de la tabla por su Id
        /// </summary>
        /// <typeparam name="T">Tipo del objeto</typeparam>
        /// <param name="repo">Objetos del repositorio</param>
        /// <param name="id">Identificador del objeto</param>
        /// <returns>El objeto si es encontrado o nulo en caso de que haya algún problema</returns>
        protected async Task<T?> GetByIdAsync<T>(IGenericRepository<T> repo, int id) where T : class
        {
            T? resultado = null;
            try
            {
                resultado = await repo.GetByIdAsync(id);
            }
            catch (DataAccessException dae)
            {
                // Mensaje claro para el usuario
                MensajeError.Mostrar("ACCESO A LOS DATOS", "Error de datos: No se puede acceder a los datos");
            }
 
            return resultado;
        }
        /// <summary>
        /// Añade un objeto en el repositorio especificado.
        /// </summary>
        /// <remarks>.</remarks>
        /// <typeparam name="T">El tipo de la entidad a añadir. Debe de ser una referencia a un tipo</typeparam>
        /// <param name="repo">El repositorio asociado al tipo de la entidad</param>
        /// <param name="entity">La entidad u objetoa a añadir al repositorio.</param>
        /// <returns>Devuelve el resultado de la operación:
        ///             true en caso de que todo haya ido bien
        ///             false en caso de que se haya producio un error</returns>
        protected async Task<bool> AddAsync<T>(IGenericRepository<T> repo, T entity) where T : class
        {
            bool resultado = true;
            try
            {
                await repo.AddAsync(entity);
            }
            catch (DataAccessException dae)
            {
                resultado = false;
            }

            return resultado;
        }
        /// <summary>
        /// Actualiza un objeto en el repositorio especificado.
        /// </summary>
        /// <typeparam name="T">El tipo de la entidad a actualizar. Debe de ser una referencia a un tipo</typeparam>
        /// <param name="repo">El repositorio asociado al tipo de la entidad</param>
        /// <param name="entity">La entidad u objetoa a actualizar al repositorio.</param>
        /// <returns>Devuelve el resultado de la operación:
        ///             true en caso de que todo haya ido bien
        ///             false en caso de que se haya producio un error</returns>
        protected async Task<bool> UpdateAsync<T>(IGenericRepository<T> repo, T entity) where T : class
        {
            bool resultado = true;
            try
            {
                await repo.UpdateAsync(entity);
            }
            catch (DataAccessException dae)
            {
                resultado = false;
            }
            return resultado;
        }
        /// <summary>
        /// Borra un objeto del repositorio especificado.
        /// </summary>
        /// <typeparam name="T">El tipo de la entidad a borrar. Debe de ser una referencia a un tipo</typeparam>
        /// <param name="repo">El repositorio asociado al tipo de la entidad</param>
        /// <param name="id">Identificador del objeto</param>
        /// <returns>Devuelve el resultado de la operación:
        ///             true en caso de que todo haya ido bien
        ///             false en caso de que se haya producio un error</returns>
        protected async Task<bool> DeleteAsync<T>(IGenericRepository<T> repo, int id) where T : class
        {
            bool resultado = true;
            try
            {
                await repo.RemoveByIdAsync(id);
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;
        }
        /// <summary>
        /// Añade o actualiza un objeto en el repositorio especificado.
        /// </summary>
        /// <typeparam name="T">El tipo de la entidad a añadir/actualizar. Debe de ser una referencia a un tipo</typeparam>
        /// <param name="repo">El repositorio asociado al tipo de la entidad</param>
        /// <param name="entity"></param>
        /// <returns>Devuelve el resultado de la operación:
        ///             true en caso de que todo haya ido bien
        ///             false en caso de que se haya producio un error</returns>
        protected async Task<bool> AddOrUpdateAsync<T>(IGenericRepository<T> repo, T entity) where T : class
        {
            bool resultado = true;
            try
            {
                // Obtener la propiedad "Id"
                var idProp = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null)
                    ?? typeof(T).GetProperties().FirstOrDefault(p => string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase))
                    ?? typeof(T).GetProperties().FirstOrDefault(p => p.Name.StartsWith("Id", StringComparison.OrdinalIgnoreCase))
                    ?? typeof(T).GetProperties().FirstOrDefault(p => p.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase))
                    ?? typeof(T).GetProperties().FirstOrDefault(p => string.Equals(p.Name, typeof(T).Name + "Id", StringComparison.OrdinalIgnoreCase));
                // Si no se encuentra la propiedad Id, no se puede continuar
                if (idProp == null)
                {
                    resultado = false;
                }
                else
                {
                    // Obtener el valor de la propiedad Id
                    var idValue = idProp.GetValue(entity);
                    // Comprobar si el valor no es nulo y es un entero
                    if (idValue != null && idValue.GetType() == typeof(int))
                    {
                        // Intentar obtener el objeto existente por su Id
                        var existing = await repo.GetByIdAsync((int)idValue);
                        if (existing == null)
                        {
                            // No existe, se añade
                            await repo.AddAsync(entity);
                        }
                        else
                        {
                            // Existe, se actualiza
                            await repo.UpdateAsync(entity);
                        }
                        await repo.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;
        }

    }
}
