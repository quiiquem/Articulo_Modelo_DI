using articulomodelo.Backend.Modelo;
using articulomodelo.Backend.Servicios;
using articulomodelo.Frontend.ControlUsuario;
using articulomodelo.Frontend.Dialogos;
using articulomodelo.MVVM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace articulomodelo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private DiinventarioexamenContext _contexto;
        /// Propiedad para almacenar el proveedor de servicios
        private IServiceProvider _serviceProvider;
        /// <summary>
        /// Constructor de la clase App
        /// </summary>
        public App()
        {
            // Configurar el contenedor de inyección de dependencias
            var serviceCollection = new ServiceCollection();
            // Configurar los servicios
            ConfigureServices(serviceCollection);
            // Construir el proveedor de servicios
            _serviceProvider = serviceCollection.BuildServiceProvider();
            _contexto = new DiinventarioexamenContext();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            // Configurar el contexto de la base de datos
            services.AddDbContext<DiinventarioexamenContext>();

            // Configurar el servicio de logging
            services.AddLogging(configure => configure.AddConsole());

            // ⚠️ SOLO registra las clases concretas con sus loggers específicos
            services.AddScoped<UsuarioRepository>(sp =>
                new UsuarioRepository(
                    sp.GetRequiredService<DiinventarioexamenContext>(),
                    sp.GetRequiredService<ILogger<GenericRepository<Usuario>>>()
                ));

            services.AddScoped<ArticuloRepository>(sp =>
                new ArticuloRepository(
                    sp.GetRequiredService<DiinventarioexamenContext>(),
                    sp.GetRequiredService<ILogger<GenericRepository<Articulo>>>()
                ));

            services.AddScoped<EspacioRepository>(sp =>
                new EspacioRepository(
                    sp.GetRequiredService<DiinventarioexamenContext>(),
                    sp.GetRequiredService<ILogger<GenericRepository<Espacio>>>()
                ));

            services.AddScoped<ModeloArticuloRepository>(sp =>
                new ModeloArticuloRepository(
                    sp.GetRequiredService<DiinventarioexamenContext>(),
                    sp.GetRequiredService<ILogger<GenericRepository<Modeloarticulo>>>()
                ));

            services.AddScoped<TipoArticuloRepository>(sp =>
                new TipoArticuloRepository(
                    sp.GetRequiredService<DiinventarioexamenContext>(),
                    sp.GetRequiredService<ILogger<GenericRepository<Tipoarticulo>>>()
                ));

            services.AddScoped<TipoUsuarioRepository>(sp =>
          new TipoUsuarioRepository(
              sp.GetRequiredService<DiinventarioexamenContext>(),
              sp.GetRequiredService<ILogger<GenericRepository<Tipousuario>>>()
          ));

            services.AddScoped<DepartamentoRepository>(sp =>
                new DepartamentoRepository(
                    sp.GetRequiredService<DiinventarioexamenContext>(),
                    sp.GetRequiredService<ILogger<GenericRepository<Departamento>>>()
                ));

            services.AddScoped<RolRepository>(sp =>
                new RolRepository(
                    sp.GetRequiredService<DiinventarioexamenContext>(),
                    sp.GetRequiredService<ILogger<GenericRepository<Rol>>>()
                ));

            services.AddScoped<GrupoRepository>(sp =>
               new GrupoRepository(
                   sp.GetRequiredService<DiinventarioexamenContext>(),
                   sp.GetRequiredService<ILogger<GenericRepository<Grupo>>>()
               ));

            // MainWindow debería ser Transient, no Singleton
            services.AddTransient<MainWindow>();

            // Registrar ViewModels
            services.AddTransient<MVArticulo>();
            services.AddTransient<VMModeloArticulo>();
            services.AddTransient<VMUsuario>();

            // Registrar dialogos
            services.AddTransient<Login>();
            services.AddTransient<DialogoModeloArticulo>();
            services.AddTransient<DialogoArticulo>();
            services.AddTransient<DialogoUsuarioNuevo>();
          
            //Registrar controles de usuario
            services.AddTransient<UCListadoModelo>();
            services.AddTransient<UCUsuarios>();
            services.AddTransient<UCArticulos>();
            //Loggin
            services.AddLogging();
        }


        //Poner Login como ventana de inicio
        protected override void OnStartup(StartupEventArgs e)
        {
            // Se genera la ventana de Login
            var loginWindow = _serviceProvider.GetService<Login>();
            loginWindow.Show();
            base.OnStartup(e);
        }
    }
}
