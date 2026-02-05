using articulomodelo.Backend.Modelo;
using articulomodelo.Frontend.Dialogos;
using articulomodelo.MVVM;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;

namespace articulomodelo.Frontend.ControlUsuario
{
    /// <summary>
    /// Interaction logic for UCListadoModelo.xaml
    /// </summary>
    public partial class UCListadoModelo : UserControl
    {
        private VMModeloArticulo _vmModeloArticulo;
        private IServiceProvider _serviceProvider;
        public UCListadoModelo(VMModeloArticulo vmModeloArticulo, IServiceProvider serviceProvider)
        {
            InitializeComponent();
           _vmModeloArticulo = vmModeloArticulo;
            _serviceProvider = serviceProvider;
        }

        private async void usuario_listaAM_loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await _vmModeloArticulo.InicializarModelosArticulos();
            await _vmModeloArticulo.InicializaTipoArticulo();
            DataContext = _vmModeloArticulo;
        }

        private void Filtrar_Click(object sender, RoutedEventArgs e)
        {
            _vmModeloArticulo.Filtrar();
        }
    
        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            _vmModeloArticulo.LimpiarFiltros();
        }

        private async void EditarModelo_Click(object sender, RoutedEventArgs e)
        {
            if (dgModelosArticulos.SelectedItem is Modeloarticulo modeloSeleccionado)
            {
                var dialogo = _serviceProvider.GetRequiredService<DialogoModeloArticulo>();
                await dialogo.Initialized(modeloSeleccionado);
                dialogo.ShowDialog();

                if (dialogo.DialogResult == true)
                {
                    await _vmModeloArticulo.Inicializa();
                }
            }
        }

        private async void EliminarModelo_Click(object sender, RoutedEventArgs e)
        {
            if (dgModelosArticulos.SelectedItem is Modeloarticulo modeloSeleccionado)
            {
                bool eliminado = await _vmModeloArticulo.EliminarModeloArticuloAsync(
                    modeloSeleccionado.Idmodeloarticulo);

                if (eliminado)
                {
                    await _vmModeloArticulo.Inicializa();
                }
            }
        }
    }
    }

