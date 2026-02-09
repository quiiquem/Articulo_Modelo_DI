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
    /// Interaction logic for UCDepartamentos.xaml
    /// </summary>
    public partial class UCDepartamentos : UserControl  
    {

        private VMDepartamento _vmDepartamento;
        public UCDepartamentos(VMDepartamento vmDepartamento)
        {
            InitializeComponent();
            _vmDepartamento = vmDepartamento;
        }

        private async void ucDepartamento_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _vmDepartamento;
            await _vmDepartamento.InicializarDepartamentos();
        }
    }
}
