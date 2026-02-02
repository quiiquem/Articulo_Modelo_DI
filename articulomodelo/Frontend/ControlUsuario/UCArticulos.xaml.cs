using articulomodelo.MVVM;
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

namespace articulomodelo.Frontend.ControlUsuario
{
    /// <summary>
    /// Interaction logic for UCArticulos.xaml
    /// </summary>
    public partial class UCArticulos : UserControl
    {
        private MVArticulo _vmArticulo;
        public UCArticulos(MVArticulo vmArticulo)
        {
            InitializeComponent();
            _vmArticulo = vmArticulo;
        }

        private async void usuario_listaArticulos_Loaded(object sender, RoutedEventArgs e)
        {
            await _vmArticulo.InicializarArticulos();
            await _vmArticulo.InicializarEspacios();    
            DataContext = _vmArticulo;
        }

        private void Filtrar_Click(object sender, RoutedEventArgs e)
        {
            _vmArticulo.Filtrar();
        }


        private void Limpiar_Click(object sender, RoutedEventArgs e)
        {
            _vmArticulo.LimpiarFiltros();
        }
    }
}
