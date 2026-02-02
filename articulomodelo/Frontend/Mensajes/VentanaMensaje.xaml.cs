using System.Windows;

namespace articulomodelo.Frontend.Mensajes
{
    /// <summary>
    /// Interaction logic for VentanaMensaje.xaml
    /// </summary>
    public partial class VentanaMensaje : Window
    {
        MensajeVentana _mensajeVentana;
        public VentanaMensaje(MensajeVentana mensajeVentana)
        {
            InitializeComponent();
            _mensajeVentana = mensajeVentana;
        }

        private void ventanaDialogoMensaje_Loaded(object sender, RoutedEventArgs e)
        {
            imgMensaje.Source = _mensajeVentana.Imagen;
            tbMensaje.Text = _mensajeVentana.Cuerpo;
            tbTitulo.Text = _mensajeVentana.Titulo;
            Aceptar.Background = _mensajeVentana.ColorDistintivo;

        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
