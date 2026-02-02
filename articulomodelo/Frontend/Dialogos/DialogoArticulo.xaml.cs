using articulomodelo.Frontend.Mensajes;
using articulomodelo.MVVM;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;

namespace articulomodelo.Frontend.Dialogos
{
    /// <summary>
    /// Interaction logic for DialogoArticulo.xaml
    /// </summary>
    public partial class DialogoArticulo : MetroWindow
    {
        private readonly MVArticulo _mvArticulo; //declarar MVArticulo
        public DialogoArticulo(MVArticulo mvArticulo)
        {
            InitializeComponent();
            _mvArticulo = mvArticulo;
        }

        private async void diagArticulo_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {


                // Comprueba la validación cada que se intenta guardar
                this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(_mvArticulo.OnErrorEvent));
                // Cargar datos
                await _mvArticulo.InicializarUsuarios();
                await _mvArticulo.InicializarModelosArticulos();
                await _mvArticulo.InicializarEspacios();


                // DataContext
                DataContext = _mvArticulo;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //Botones
        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Forzar actualización de bindings
            cmbBox_Modelo.GetBindingExpression(ComboBox.SelectedValueProperty)?.UpdateSource();
            Espacios_CmbBox.GetBindingExpression(ComboBox.SelectedValueProperty)?.UpdateSource();

            // Log de debug
            System.Diagnostics.Debug.WriteLine($"Modelo: {_mvArticulo.articulo.Modelo}");
            System.Diagnostics.Debug.WriteLine($"Espacio: {_mvArticulo.articulo.Espacio}");

            if (_mvArticulo.HasErrors) // Si NO hay errores
            {
                try
                {
                    bool guardado = await _mvArticulo.GuardarArticuloAsync();

                    if (guardado)
                    {
                        MensajeInformacion.Mostrar("Artículo guardado correctamente", "Éxito");
                        DialogResult = true;
                    }
                    else
                    {
                        MensajeError.Mostrar("Error al guardar el artículo", "Error");
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


        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void txtModeloArticulo_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
