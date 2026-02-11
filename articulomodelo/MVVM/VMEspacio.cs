using articulomodelo.Backend.Modelo;
using articulomodelo.Backend.Servicios;
using articulomodelo.Frontend.Mensajes;
using articulomodelo.MVVM.Implementacion;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace articulomodelo.MVVM
{
    public class VMEspacio : MVBase
    {
        private Espacio _espacio;
        private EspacioRepository _espacioRepository;
        private List<Espacio> _listaEspacios;
        private DepartamentoRepository _departamentoRepository;
        private List<Departamento> _listaDepartamentos;
        private List<Usuario> _listaUsuarios;
        private List<Modeloarticulo> _listaModeloArticulo;
        private Articulo _articuloSeleccionado;
        private UsuarioRepository _usuarioRepository;
        private ModeloArticuloRepository _modeloarticuloRepository;
        private List<string> _listaEstados;

        public VMEspacio(
            EspacioRepository espacioRepository,
            DepartamentoRepository departamentoRepository,
            UsuarioRepository usuarioRepository,
            ArticuloRepository articuloRepository,
            ModeloArticuloRepository modeloarticuloRepository)
        {
            _usuarioRepository = usuarioRepository;
            _modeloarticuloRepository = modeloarticuloRepository;
            _espacioRepository = espacioRepository;
            _departamentoRepository = departamentoRepository;
            _espacio = new Espacio();
            _listaEspacios = new List<Espacio>();
            _listaDepartamentos = new List<Departamento>();
            _listaUsuarios = new List<Usuario>();
            _listaModeloArticulo = new List<Modeloarticulo>();

            // Inicializar lista de estados si los necesitas
            _listaEstados = new List<string> { "Activo", "Inactivo", "Mantenimiento" }; // Ejemplo
        }

        public Espacio Espacio
        {
            get => _espacio;
            set => SetProperty(ref _espacio, value);
        }

        public List<Espacio> listaEspacios
        {
            get => _listaEspacios;
            set => SetProperty(ref _listaEspacios, value);
        }

        public List<Departamento> ListaDepartamentos
        {
            get => _listaDepartamentos;
            set => SetProperty(ref _listaDepartamentos, value);
        }

        public List<Usuario> ListaUsuarios
        {
            get => _listaUsuarios;
            set => SetProperty(ref _listaUsuarios, value);
        }

        public List<Modeloarticulo> ListaModeloArticulo
        {
            get => _listaModeloArticulo;
            set => SetProperty(ref _listaModeloArticulo, value);
        }

        public List<string> ListaEstados
        {
            get => _listaEstados;
            set => SetProperty(ref _listaEstados, value);
        }

        public Articulo articuloSeleccionado
        {
            get => _articuloSeleccionado;
            set => SetProperty(ref _articuloSeleccionado, value);
        }

        public async Task InicializarEspacios()
        {
            try
            {
                listaEspacios = await _espacioRepository.GetAllConArticulosAsync();
                ListaUsuarios = await _usuarioRepository.GetAllAsync();
                ListaModeloArticulo = await _modeloarticuloRepository.GetAllAsync();
                ListaDepartamentos = await _departamentoRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                MensajeError.Mostrar("GESTIÓN ESPACIOS",
                    "Error al cargar los espacios\nNo puedo conectar con la base de datos", 0);
            }
        }
    }
}