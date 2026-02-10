using articulomodelo.Backend.Modelo;
using articulomodelo.Backend.Servicios;
using articulomodelo.Frontend.Mensajes;
using articulomodelo.MVVM.Implementacion;
using Microsoft.EntityFrameworkCore;
using ProyectoDI_Trimestre1.Frontend.Mensajes;
using System.Windows.Data;

namespace articulomodelo.MVVM
{
    public class MVArticulo : MVBase
    {
        #region Campos y propiedades privadas

        /// <summary>
        /// Vincular a la vista para mostrar y editar datos de articulo no modelo
        private Articulo _articulo;
        /// <summary>
        /// Repositorio para gestionar las operaciones de datos relacionadas con los artículos
        private ArticuloRepository _articuloRepository;
        /// <summary>
        /// Repositorio para gestionar las operaciones de datos relacionadas con los modelo de artículo
        private ModeloArticuloRepository _modeloarticuloRepository;
        ///<summary>
        /// Repositorio para gestionar las operaciones de datos relacionadas con los espacios
        private EspacioRepository _espacioRepository;
        /// <summary>
        /// Repositorio para gestionar las operaciones de datos relacionadas con los usuarios
        /// </summary>
        private UsuarioRepository _usuarioRepository;

        ///Repositorio de departamentos
        private DepartamentoRepository _departamentoRepository;

        //Espacio para luego filtrar
        private Espacio _espacioSeleccionado;
        /// <summary>
        /// lista de los usuarios que hay en la base de datos
        /// </summary>
        private List<Usuario> _listaUsuarios;
        /// <summary>
        /// lista de los modelos (que es required)
        private List<Modeloarticulo> _listaModelosArticulos;
        /// <summary>
        /// lista de los espacios disponibles
        private List<Espacio> _listaEspacios;
        /// <summary>
        /// lista de los departamentos
        private List<Departamento> _listaDepartamentos;
        /// </summary>
        /// lista de articulos (UserControl)
        private List<Articulo> _listaArticulos;

        public ListCollectionView listaArticulos_CollectionView { get; set; }

        //Declarar lista de filtros
        private List<Predicate<Articulo>> _criterios;

        private Predicate<Articulo> _criterioFechaAlta;
        private Predicate<Articulo> _criterioNumSerie;
        private Predicate<Articulo> _criterioEspacio;
        private Predicate<object> _predicadoFiltros;



        #endregion
        #region Getters y Setters
        public List<Usuario> listaUsuarios => _listaUsuarios;
        public List<Modeloarticulo> listaModelos => _listaModelosArticulos;
        public List<Espacio> listaEspacios => _listaEspacios;
        public List<Departamento> listaDepartamentos => _listaDepartamentos;

        public List<Articulo> listaArticulos => _listaArticulos;

        // Lista personalizada para el estado
        public List<string?> ListaEstados { get; } = new() { null, // permite estado NULL
        "Nuevo", "Usado", "Dañado" };


          //Declarar articulo
        public Articulo articulo
        {
            get => _articulo;
            set => SetProperty(ref _articulo, value);
        }

        //Declarar fecha alta (necesitamos dos para el rango)
        private DateTime? _fechaAltaDesde;
        public DateTime? FechaAltaDesde
        {
            get => _fechaAltaDesde;
            set
            {
                SetProperty(ref _fechaAltaDesde, value);
            }
        }

        private DateTime? _fechaAltaHasta;
        public DateTime? FechaAltaHasta
        {
            get => _fechaAltaHasta;
            set
            {
                SetProperty(ref _fechaAltaHasta, value);
            }
        }

        //Declarar numserie

        private string? _numSerieFiltro;
        public string? NumSerieFiltro
        {
            get => _numSerieFiltro;
            set
            {
               SetProperty(ref _numSerieFiltro, value);
            }
        }

        //Espacio Navigation
        public Espacio espacioSeleccionado
        {
            get => _espacioSeleccionado;
            set
            {
                SetProperty(ref _espacioSeleccionado, value);
            }
        }

        #endregion
        public MVArticulo(ArticuloRepository articuloRepository,
                              UsuarioRepository usuarioRepository,
                              ModeloArticuloRepository modeloarticuloRepository,
                              EspacioRepository espacioRepository,
                              DepartamentoRepository departamentoRepository)
        {
            _articuloRepository = articuloRepository;
            _usuarioRepository = usuarioRepository;
            _modeloarticuloRepository = modeloarticuloRepository;
            _espacioRepository = espacioRepository;
            _articulo = new Articulo();


            _criterios = new List<Predicate<Articulo>>();
            InicializaCriterios();
            _predicadoFiltros = new Predicate<object>(FiltroCriterios);
        }

        //-----------------
        //DIALOGO ARTICULO
        //-----------------

        
        //Listar articulos (UserControl)

        public async Task InicializarArticulos()
        {
            try
            {
                _listaArticulos = await _articuloRepository.GetAllWithRelationsAsync();
                OnPropertyChanged(nameof(listaArticulos));

                listaArticulos_CollectionView = new ListCollectionView(_listaArticulos);
                OnPropertyChanged(nameof(listaArticulos_CollectionView));
            }
            catch (Exception ex)
            {
                MensajeError.Mostrar("GESTIÓN ARTÍCULOS", "Error al cargar los artículos\n" +
                    "No puedo conectar con la base de datos", 0);
            }
        }

        //Listar usuarios (Alta / Baja usuario)
        public async Task InicializarUsuarios()
        {
            try
            {
                _listaUsuarios = await GetAllAsync<Usuario>(_usuarioRepository);
                OnPropertyChanged(nameof(listaUsuarios));
            }
            catch (Exception ex)
            {
                MensajeError.Mostrar("GESTIÓN USUARIOS", "Error al cargar los usuarios\n" +
                    "No puedo conectar con la base de datos", 0);
            }
        }

        //Listar modelos (Campo obligatorio Modelo)
        public async Task InicializarModelosArticulos()
        {
            try
            {
                _listaModelosArticulos = await GetAllAsync<Modeloarticulo>(_modeloarticuloRepository);
            }
            catch (Exception ex)
            {
                MensajeError.Mostrar("GESTIÓN MODELOS ARTÍCULOS"," Error al cargar los modelos de artículos\n" +
                    "No puedo conectar con la base de datos", 0);
            }
        }

        //Listar espacios (Campo obligatorio Espacio)
        public async Task InicializarEspacios()
        {
            try
            {
                _listaEspacios = await GetAllAsync<Espacio>(_espacioRepository);
                OnPropertyChanged(nameof(listaEspacios));
            }
            catch (Exception ex)
            {
                MensajeError.Mostrar("GESTIÓN ESPACIOS", "Error al cargar los espacios\n" +
                    "No puedo conectar con la base de datos", 0);
            }
        }


        //Listar departamentos (Campo opcional)
        public async Task IncializarDepartamentos()
        {
            try
            {
                _listaDepartamentos = await GetAllAsync<Departamento>(_departamentoRepository);
                OnPropertyChanged(nameof(listaDepartamentos));
            }
            catch (Exception ex)
            {
                MensajeError.Mostrar("GESTIÓN DEPARTAMENTOS", "Error al cargar los departamentos\n" +
                    "No puedo conectar con la base de datos", 0);
            }
        }

 

        public async Task<bool> GuardarArticuloAsync()
        {
            bool correcto = true;
            try
            {
                if (articulo.Idarticulo == 0)
                {
                    // Generar nuevo ID porque articulo NO es AI
                    int nuevoId = await _articuloRepository.ObtenerMaximoId() + 1;
                    articulo.Idarticulo = nuevoId;
                    await _articuloRepository.AddAsync(articulo);
                }
                else
                {
                    // Actualizar modelo de artículo existente
                    await _articuloRepository.UpdateAsync(articulo);
                }
            }
            catch (Exception ex)
            {
                // Capturamos la excepción y la registramos en el log
                correcto = false;
            }
            return correcto;
        }

        //Todo lo de filtros

        #region Filtrar Articulos

        private void InicializaCriterios()
        {
            _criterioNumSerie = new Predicate<Articulo>(m =>
            string.IsNullOrEmpty(NumSerieFiltro) || 
            (!string.IsNullOrEmpty(m.Numserie) && m.Numserie.ToLower().StartsWith(NumSerieFiltro.ToLower()))
        );

            _criterioEspacio = new Predicate<Articulo>(m =>
                espacioSeleccionado == null || 
                (m.tipoEspacioSeleccionado != null && m.tipoEspacioSeleccionado.Equals(espacioSeleccionado))
            );

            _criterioFechaAlta = new Predicate<Articulo>(m =>
     (FechaAltaDesde == null && FechaAltaHasta == null) ||  (m.Fechaalta != null &&  
     (FechaAltaDesde == null || m.Fechaalta >= FechaAltaDesde) &&
      (FechaAltaHasta == null || m.Fechaalta <= FechaAltaHasta))
 );
        }


        private void AddCriterios()
        {
            _criterios.Clear();

            if (_criterioFechaAlta != null)
                _criterios.Add(_criterioFechaAlta);

            if(_criterioNumSerie != null)
                _criterios.Add(_criterioNumSerie);

            if (_criterioEspacio != null)
                _criterios.Add(_criterioEspacio);
        }

        private bool FiltroCriterios(object item)
        {
            bool correcto = true;
            Articulo articulo = (Articulo)item;

            if (_criterios != null)
            {
                correcto = _criterios.TrueForAll(x => x(articulo));
            }

            return correcto;
        }

        public void Filtrar()
        {
            InicializaCriterios();
            AddCriterios();
            listaArticulos_CollectionView.Filter = _predicadoFiltros;
        }

        public void LimpiarFiltros()
        {
            NumSerieFiltro = null;
            espacioSeleccionado = null;
            FechaAltaDesde = null;
            FechaAltaHasta = null;
            listaArticulos_CollectionView.Filter = null;
            listaArticulos_CollectionView.Refresh();
        }

        #endregion
    }
}
