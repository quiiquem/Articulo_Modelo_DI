using articulomodelo.Backend.Modelo;
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
    /// Interaction logic for UCAdministracionEspacios.xaml
    /// </summary>
    public partial class UCAdministracionEspacios : UserControl

    {
        private VMEspacio _vmEspacio;
        public UCAdministracionEspacios(VMEspacio vmEspacio)
        {
            InitializeComponent();
            _vmEspacio = vmEspacio;
        }

        private async void ucArbolEspacio_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _vmEspacio;
         await _vmEspacio.InicializarEspacios();
        }

        private void treeEspacio_Cambio(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(treeEspacios.SelectedItem is Espacio)
            {
                dgArticuloPorEspacio.ItemsSource = ((Espacio)treeEspacios.SelectedItem).Articulos;
            }
        }
    }
}
