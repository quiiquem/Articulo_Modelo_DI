using articulomodelo.Backend.Modelo;
using articulomodelo.Frontend.Mensajes;
using articulomodelo.MVVM;
using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace articulomodelo.Frontend.Dialogos
{
    public partial class DialogoModeloArticulo : MetroWindow
    {
        private readonly VMModeloArticulo _vmModeloArticulo;

        public DialogoModeloArticulo(VMModeloArticulo mvArticulo)
        {
            InitializeComponent();
            _vmModeloArticulo = mvArticulo;
        }

        public async Task Inicializa(Modeloarticulo modeloarticulo)
        {
            await _vmModeloArticulo.Inicializa();
            _vmModeloArticulo.modeloArticulo = modeloarticulo;
            this.RemoveHandler(Validation.ErrorEvent, new RoutedEventHandler(_vmModeloArticulo.OnErrorEvent)); //reiniciar los errores porque si no cada que se crea/edita un modelo visual studio decide irse 
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(_vmModeloArticulo.OnErrorEvent));
            DataContext = _vmModeloArticulo;
        }

        private async void btnAnyadirModeloArticulo_Click(object sender, RoutedEventArgs e)
        {
            if (_vmModeloArticulo.HasErrors)
            {
                try
                {
                    btnAnyadirModeloArticulo.IsEnabled = true;
                    bool guardado = await _vmModeloArticulo.GuardarModeloArticuloAsync();

                    if (guardado)
                    {
                        MensajeInformacion.Mostrar("Modelo de artículo guardado correctamente", "Éxito");
                        DialogResult = true;
                    }
                }
                catch (Exception ex)
                {
                    MensajeAdvertencia.Mostrar("Ha habido problemas con el servidor", "Error con el servidor");
                }
            }
            else
            {
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