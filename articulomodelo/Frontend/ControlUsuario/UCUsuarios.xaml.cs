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
    /// Interaction logic for UCAdministracion.xaml
    /// </summary>
    public partial class UCUsuarios : UserControl
    {
        private VMUsuario _vmUsuario;
        public UCUsuarios(VMUsuario vmUsuario)
        {
            InitializeComponent();
            _vmUsuario = vmUsuario;
        }


        //TODO: Mover lo del overlay de carga al proyecto intermodular, aqui no es 'necesario' (no lo ha pedido)
        private async void usuario_listaUsuario_loaded(object sender, RoutedEventArgs e)
        {
           // OverlayCargando.Visibility = Visibility.Visible; 
            //await Task.Delay(50);
            await _vmUsuario.InicializarUsuarios();
            this.DataContext = _vmUsuario;

            //OverlayCargando.Visibility = Visibility.Collapsed; 
        }
    }
}
