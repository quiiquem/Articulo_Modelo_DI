using articulomodelo.Frontend.ControlUsuario;
using articulomodelo.Frontend.Dialogos;
using articulomodelo.MVVM;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using System.Windows;

namespace articulomodelo
{
    public partial class MainWindow : MetroWindow
    {
        private readonly VMModeloArticulo _mvModeloArticulo;
        private readonly MVArticulo _mvArticulo;
        private readonly VMUsuario _mvUsuario;
        private UCListadoModelo _listadoArticuloModelo;
        private UCUsuarios _listadoUsuarios;
        private UCArticulos _listadoArticulos;

        //Constructor MainWindow con las ventanas dialogo inyectadas (ahora si podemos pues pusimos los servicios en App.xaml.cs)
        public MainWindow(MVArticulo mvArticulo, VMModeloArticulo mvModeloArticulo,
            VMUsuario mvUsuario, UCListadoModelo listadoArticuloModelo, UCUsuarios listadoUsuarios,
            UCArticulos listadoArticulos)
        {
            InitializeComponent();
            _mvModeloArticulo = mvModeloArticulo;
            _mvArticulo = mvArticulo;
            _mvUsuario = mvUsuario;
            _listadoArticuloModelo = listadoArticuloModelo;
            _listadoUsuarios = listadoUsuarios;
            _listadoArticulos = listadoArticulos;
        }

        //Botones de la barra horizontal azul de arriba
        private void min_window_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; //maximizar la ventana
        }

        private void max_window_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized; //maximizar la ventana
            }
            else
            {
                this.WindowState = WindowState.Normal; //restaurar la ventana
            }
        }
        private void salir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); //cerrar la aplicación
        }

        //Botones que abren dialogos (Crear MA, Articulo y Usuario)
        //Crear Modelo Articulo
        private void Crear_Click(object sender, RoutedEventArgs e) //Ventana Dialogo Crear Articulo
        {

            //Generar una nueva cada que se inicia para que VM no explote
            var dialogo = new DialogoModeloArticulo(_mvModeloArticulo); 
            dialogo.ShowDialog();
        }

        private void Crear_Articulo(object sender, RoutedEventArgs e) //Ventana Dialogo Crear Articulo
        {

            //Generar una nueva cada que se inicia para que VM no explote
            var dialogo = new DialogoArticulo(_mvArticulo);
            dialogo.ShowDialog();
        }

        private void Crear_Usuario(object sender, RoutedEventArgs e) //Ventana Dialogo Crear Usuario
        {
            var dialogo = new DialogoUsuarioNuevo(_mvUsuario);
            dialogo.ShowDialog();
        }


        //Botones que abren control usuarios (Datagrids)

        private void UsuarioControl_ModeloArticulo(object sender, RoutedEventArgs e) //Ventana Control ArticuloModelo
        {
            Ventana_Principal.Children.Clear();
            Ventana_Principal.Children.Add(_listadoArticuloModelo);
        }



        private void UsuarioControl_Usuario(object sender, RoutedEventArgs e) //Ventana Control Usuarios
        {
            Ventana_Principal.Children.Clear();
            Ventana_Principal.Children.Add(_listadoUsuarios);
        }

      
        private void UsuarioControl_Articulo(object sender, RoutedEventArgs e) //Ventana Control Articulos
        {
            Ventana_Principal.Children.Clear();
            Ventana_Principal.Children.Add(_listadoArticulos);
        }



    }
}