using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace articulomodelo.MVVM.Implementacion
{
    public abstract class ValidatableViewModel : BaseViewModel, IDataErrorInfo
    {
        // Implementación de IDataErrorInfo
        /// <summary>
        /// Propiedad que indica si el objeto es válido o no
        /// En principio devuelve null
        /// </summary>
        public virtual string Error => null!;
        /// <summary>
        /// Propiedad que permite validar las propiedades del objeto
        /// </summary>
        /// <param name="columnName">Nombre de la propiedad o atributo del objeto </param>
        /// <returns>Devuelve el primer mensaje de error que encuentra.
        ///          Si no hay errores, entonces devuelve la cadena vacía</returns>
        public virtual string this[string columnName]
        {
            get
            {
                System.Diagnostics.Debug.WriteLine($"========================================");
                System.Diagnostics.Debug.WriteLine($"🔍 VALIDANDO COLUMNA: {columnName}");
                System.Diagnostics.Debug.WriteLine($"   Tipo del objeto: {this.GetType().Name}");

                var property = GetType().GetProperty(columnName);

                if (property == null)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ PROPIEDAD NO ENCONTRADA: {columnName}");
                    System.Diagnostics.Debug.WriteLine($"========================================");
                    return null;
                }

                var value = property.GetValue(this);
                System.Diagnostics.Debug.WriteLine($"   Valor actual: {value ?? "NULL"}");
                System.Diagnostics.Debug.WriteLine($"   Tipo del valor: {value?.GetType().Name ?? "NULL"}");

                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(this) { MemberName = columnName };

                System.Diagnostics.Debug.WriteLine($"   Intentando validar...");

                bool isValid = Validator.TryValidateProperty(value, validationContext, validationResults);

                if (isValid)
                {
                    System.Diagnostics.Debug.WriteLine($"✅ VALIDACIÓN EXITOSA - Sin errores");
                    System.Diagnostics.Debug.WriteLine($"========================================");
                    return null;
                }

                var error = validationResults.First().ErrorMessage;
                System.Diagnostics.Debug.WriteLine($"❌ VALIDACIÓN FALLIDA");
                System.Diagnostics.Debug.WriteLine($"   Error: {error}");
                System.Diagnostics.Debug.WriteLine($"========================================");

                return error;
            }
        }

        /// <summary>
        /// Método que debe ser sobreescrito en los ViewModels derivados para validar propiedades.
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad a validar</param>
        /// <returns>Mensaje de error o string vacío si no hay error</returns>
        // Puedes seguir sobrescribiendo este método en los hijos si necesitas validaciones adicionales
        protected virtual string ValidateProperty(string propertyName) => string.Empty;

        /// <summary>
        /// Método para notificar a la vista que debe reevaluar la disponibilidad de los comandos.
        /// </summary>
        protected void RaiseCommandsCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
