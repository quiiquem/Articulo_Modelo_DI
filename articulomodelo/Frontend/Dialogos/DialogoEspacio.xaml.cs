using articulomodelo.Frontend.Mensajes;
using articulomodelo.MVVM;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace articulomodelo.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DialogoEspacio.xaml
    /// </summary>
    public partial class DialogoEspacio : MetroWindow
    {
        private readonly VMEspacio _vmEspacio; //declarar ViewModel

        public DialogoEspacio(VMEspacio vmEspacio)
        {
            InitializeComponent();
            _vmEspacio = vmEspacio;
        }


        private async void dialogoEspacio_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Cargar datos
                this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(_vmEspacio.OnErrorEvent));


                // DataContext
                DataContext = _vmEspacio;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void btnGuardarEspacio_Click(object sender, RoutedEventArgs e)
        {

            if (_vmEspacio.HasErrors) // Si NO hay errores
            {
                try
                {
                    bool guardado = await _vmEspacio.GuardarEspacioAsync();

                    if (guardado)
                    {
                        MensajeInformacion.Mostrar("Espacio guardado correctamente", "Éxito");
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

        private void btnCancelarEspacio_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }

}
