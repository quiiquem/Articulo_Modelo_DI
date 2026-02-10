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
    /// Interaction logic for UCGrupos.xaml
    /// </summary>
    public partial class UCGrupos : UserControl
    {

        private VMGrupo _vmGrupo;

        public UCGrupos(VMGrupo vmGrupo)
        {
            InitializeComponent();
            _vmGrupo = vmGrupo;
        }

        private async void ucGrupo_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _vmGrupo;
            await _vmGrupo.InicializarGrupos_Arbol();
        }
    }
}
