using articulomodelo.Frontend.Mensajes;
using articulomodelo.MVVM;
using MahApps.Metro.Controls;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace articulomodelo.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DialogoUsuarioNuevo.xaml
    /// </summary>
    public partial class DialogoUsuarioNuevo : MetroWindow
    {
        private readonly VMUsuario _mvUsuario;

        public DialogoUsuarioNuevo(VMUsuario mvUsuario)
        {
            InitializeComponent();
            _mvUsuario = mvUsuario;
        }


        private async void diagUsuario_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(_mvUsuario.OnErrorEvent));

                await _mvUsuario.InicializarDepartamentos();
                await _mvUsuario.InicializarRoles();
                await _mvUsuario.InicializarGrupos();
                await _mvUsuario.InciializarTipoUsuario();

                DataContext = _mvUsuario;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"💥 ERROR EN CARGA: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private void TestValidacion_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("=== TEST DE VALIDACIÓN ===");
            System.Diagnostics.Debug.WriteLine($"Usuario.Username: '{_mvUsuario.usuario.Username ?? "NULL"}'");
            System.Diagnostics.Debug.WriteLine($"HasErrors: {_mvUsuario.HasErrors}");

            // Forzar validación manual
            var error = _mvUsuario["usuario.Username"];
            System.Diagnostics.Debug.WriteLine($"Error manual: {error ?? "SIN ERROR"}");
        }

        private async void guardar_usuario_Click(object sender, RoutedEventArgs e)
        {
            nombre_usuario.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            tipo_usuario.GetBindingExpression(ComboBox.SelectedItemProperty)?.UpdateSource();
            grupo.GetBindingExpression(ComboBox.SelectedItemProperty)?.UpdateSource();
            domicilio.GetBindingExpression(ComboBox.SelectedItemProperty)?.UpdateSource();
            rol.GetBindingExpression(ComboBox.SelectedItemProperty)?.UpdateSource();

            // Para password (si el helper lo soporta)
            contra_usuario.GetBindingExpression(PasswordBox.PasswordCharProperty)?.UpdateSource();

            if (_mvUsuario.HasErrors) // Si NO hay errores
            {
                try
                {
                    bool guardado = await _mvUsuario.GuardarUsuarioAsync();

                    if (guardado)
                    {
                        MensajeInformacion.Mostrar("Usuario guardado correctamente", "Éxito");
                        DialogResult = true;
                    }
                    else
                    {
                        MensajeError.Mostrar("Error al guardar el usuario", "Error");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
                    MensajeAdvertencia.Mostrar("Ha habido problemas con el servidor", "Error");
                }
            }
            else
            {
                MensajeError.Mostrar("Por favor, corrija los errores antes de guardar", "Error de validación");
            }
        }

        private void cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

 
    }
}