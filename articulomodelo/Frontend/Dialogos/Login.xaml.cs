using articulomodelo.Backend.Modelo;
using articulomodelo.Backend.Servicios;
using articulomodelo.Frontend.Mensajes;
using articulomodelo.MVVM;
using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;

namespace articulomodelo.Frontend.Dialogos
{

    public partial class Login : MetroWindow
    {

        //Añadir ventanas en readonly
        private readonly UsuarioRepository _usuarioRepository;
        private readonly MainWindow _mainWindow;

        public Login(UsuarioRepository usuarioRepository,
                    MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _usuarioRepository = usuarioRepository;
        }

        private void btn_login_Click(object sender, RoutedEventArgs e) //Accion de dar al botón de login
        {

        
            if (string.IsNullOrWhiteSpace(txt_user.Text) || string.IsNullOrWhiteSpace(txt_password.Password))
            {
                MensajeAdvertencia.Mostrar("Por favor, rellene todos los campos.", "Warning");
                
                return;
            }

            string usuario = txt_user.Text.Trim(); //vaciarme los campos
            string password = txt_password.Password.Trim();

            if (ValidarLogin(usuario, password)) //Situacion correcta, funciona
            {
                MensajeInformacion.Mostrar("Login correcto,\nbienvenido " + usuario, "LOGIN EXITOSO");
                //Abrir mainwindow
                _mainWindow.Show();
                this.Close();
            }
            else //Datos incorrectos, no funciona
            {
                MensajeError.Mostrar("Usuario o contraseña incorrectos,\ninserte su usuario e contraseña de nuevo", "ERROR DE LOGIN");
            }
        }

        private bool ValidarLogin(string usuario, string password)
        {
            using (var db = new DiinventarioexamenContext())
            {
                // Solo selecciona los campos necesarios, evitando cargar relaciones
                var user = db.Usuario
                    .Where(u => u.Username == usuario && u.Password == password)
                    .Select(u => new { u.Username })  // Solo necesitas saber si existe
                    .FirstOrDefault();

                return user != null;  // Si encontró algo, login correcto
            }
        }


    }
}
