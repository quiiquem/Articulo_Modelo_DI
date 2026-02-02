using System.Windows;
using System.Windows.Controls;

// RESUMEN: Este helper permite enlazar (binding) la contraseña de un PasswordBox con una propiedad en el ViewModel (patrón MVVM).
// Sin esto, tendríamos que usar un TextBox (inseguro, muestra la contraseña) o manejar la contraseña manualmente en code-behind.
// Con este helper, podemos hacer: helpers:PasswordBoxHelper.BoundPassword="{Binding usuario.Password}"

namespace articulomodelo.Helpers
{
    public static class PasswordBoxHelper
    {
        // 1. DECLARACIÓN DE LA PROPIEDAD ADJUNTA (Attached Property)
        // Esto crea una propiedad "BoundPassword" que se puede usar en XAML en cualquier PasswordBox
        // Es como agregar una nueva propiedad al PasswordBox sin modificar la clase original
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundPassword",                    // Nombre de la propiedad
                typeof(string),                     // Tipo de dato (string para la contraseña)
                typeof(PasswordBoxHelper),          // Clase propietaria
                new FrameworkPropertyMetadata(
                    string.Empty,                   // Valor por defecto (vacío)
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,  // Permite binding bidireccional automático
                    OnBoundPasswordChanged));       // Método que se ejecuta cuando cambia el valor desde el ViewModel

        // 2. GETTER - Obtiene el valor de BoundPassword desde el PasswordBox
        // WPF lo usa internamente para leer el valor cuando hace binding
        public static string GetBoundPassword(DependencyObject d)
        {
            return (string)d.GetValue(BoundPasswordProperty);
        }

        // 3. SETTER - Establece el valor de BoundPassword en el PasswordBox
        // WPF lo usa internamente para escribir el valor cuando hace binding
        public static void SetBoundPassword(DependencyObject d, string value)
        {
            d.SetValue(BoundPasswordProperty, value);
        }

        // 4. EVENTO: Cuando el ViewModel cambia la contraseña, actualiza el PasswordBox
        // Este método se ejecuta cuando la propiedad en el ViewModel cambia (ViewModel → Vista)
        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                // Desuscribimos temporalmente el evento para evitar un bucle infinito
                // (si no, al cambiar Password se dispararía PasswordBox_PasswordChanged, que volvería a disparar esto)
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

                // Solo actualizamos si el valor realmente cambió (optimización)
                if (!string.Equals(passwordBox.Password, e.NewValue))
                {
                    // Actualizamos la contraseña visual del PasswordBox con el valor del ViewModel
                    passwordBox.Password = (e.NewValue ?? string.Empty).ToString();
                }

                // Volvemos a suscribir el evento para capturar futuros cambios del usuario
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        // 5. EVENTO: Cuando el usuario escribe en el PasswordBox, actualiza el ViewModel
        // Este método se ejecuta cada vez que el usuario escribe o borra caracteres (Vista → ViewModel)
        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                // Actualizamos la propiedad BoundPassword con lo que el usuario escribió
                // Esto dispara el binding y actualiza automáticamente la propiedad en el ViewModel
                SetBoundPassword(passwordBox, passwordBox.Password);
            }
        }
    }
}

// FLUJO COMPLETO:
// 
// Usuario escribe "abc123" en PasswordBox →
// Se dispara PasswordBox_PasswordChanged →
// Llama a SetBoundPassword(passwordBox, "abc123") →
// El binding actualiza usuario.Password = "abc123" en el ViewModel ✅
//
// Si el ViewModel cambia usuario.Password = "nueva" →
// Se dispara OnBoundPasswordChanged →
// Actualiza passwordBox.Password = "nueva" →
// El usuario ve "******" en pantalla ✅