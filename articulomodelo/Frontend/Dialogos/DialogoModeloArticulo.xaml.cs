using articulomodelo.Backend.Modelo;
using articulomodelo.Frontend.Mensajes;
using articulomodelo.MVVM;
using articulomodelo.MVVM.Implementacion;
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
    /// Interaction logic for DialogoModeloArticulo.xaml
    /// </summary>
    public partial class DialogoModeloArticulo : MetroWindow
    {
        private readonly VMModeloArticulo _vmModeloArticulo; //declarar MVArticulo
            public DialogoModeloArticulo(VMModeloArticulo mvArticulo)
            {
                InitializeComponent();
            _vmModeloArticulo = mvArticulo;
          
        }

        public async Task Initialized(Modeloarticulo modeloarticulo)
        {
                await _vmModeloArticulo.Inicializa();
                _vmModeloArticulo.modeloArticulo = modeloarticulo;
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(_vmModeloArticulo.OnErrorEvent));
            DataContext = _vmModeloArticulo;
        }


        //BOTONES por activar

        //Boton de guardar modelo artículo
        private async void btnAnyadirModeloArticulo_Click(object sender, RoutedEventArgs e) // Guardar Modelo Artículo con metodo async
        {
         
           if (_vmModeloArticulo.HasErrors) //Si no hay errores de validación
            {
                try
                {
                    btnAnyadirModeloArticulo.IsEnabled = true; //Reactivar boton guardar

                    //Decirle al MVArticulo que guarde el modelo de artículo (usando el codigo que tiene VMArticulo)
                    bool guardado = await _vmModeloArticulo.GuardarModeloArticuloAsync(); //Guardar MA en la BD
                    if (guardado)
                    {
                        MensajeInformacion.Mostrar("Modelo de artículo guardado correctamente",
                                             "Éxito");
                        DialogResult = true; // cerrar ventana indicando éxito
                    }
                }
                catch (Exception ex)
                {
                    MensajeAdvertencia.Mostrar("Ha habido problemas con el servidor ", "Error con el servidor");
                }
            } else {
                MensajeError.Mostrar("Por favor, corrija los errores antes de guardar el modelo de artículo.", "Error de validación");
                
            }
        }

        private void btnCancelarModeloArticulo_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
