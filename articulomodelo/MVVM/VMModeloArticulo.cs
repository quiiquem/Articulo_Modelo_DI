using articulomodelo.Backend.Modelo;
using articulomodelo.Backend.Servicios;
using articulomodelo.Frontend.Mensajes;
using articulomodelo.MVVM.Implementacion;
using ProyectoDI_Trimestre1.Frontend.Mensajes;
using System.Windows.Data;

namespace articulomodelo.MVVM
{
    public class VMModeloArticulo : MVBase
    {
            #region Campos y propiedades privadas
            /// <summary>
            /// Objeto que guarda el modelo de artículo actual
            /// Está vinculado a la vista para mostrar y editar los datos del artículo
            /// </summary>
            private Modeloarticulo _modeloArticulo;
        /// <summary>
        /// Vincular a la vista para mostrar y editar datos de articulo no modelo
        /// </summary>
              private ModeloArticuloRepository _modeloArticuloRepository;
            /// <summary>
            /// Repositorio para gestionar las operaciones de datos relacionadas con los tipos de artículo
            /// </summary>
            private TipoArticuloRepository _tipoArticuloRepository;

        /// <summary>
        /// lista de tipos de artículos disponibles
        /// </summary>
        private List<Tipoarticulo> _listaTipoArticulos;
        /// <summary>
        /// <summary>
        //tipoarticulo para que sea seleccionable
        /// </summary>
        private Tipoarticulo _tipoNavigationSelecionado;
       /// <summary>
       /// lista de los usuarios que hay en la base de datos
       /// </summary>
       private List<Usuario> _listaUsuarios;

        /// <summary>
        /// lista de modelo articulos de la BD (por esto no cargaba)
        private List<Modeloarticulo> _listaModelosArticulos;

        //Lista publica de collectionview para filtrar resultados
        public ListCollectionView listaModelo_CollectionView { get; set; }

        //lista para el filtro
        private List<Predicate<Modeloarticulo>> _criterios;
        private Predicate<Modeloarticulo> _criterioTipoNavigation;
        private Predicate<object> _predicadoFiltros;

        #endregion


        #region Getters y Setters
        
        public List<Modeloarticulo> listaModelos => _listaModelosArticulos;
        public List<Tipoarticulo> listaTiposArticulos => _listaTipoArticulos;
        public List <Usuario> listaUsuarios => _listaUsuarios;

        //Declarar modelo articulo
        public Modeloarticulo modeloArticulo
            {
                get => _modeloArticulo;
                set => SetProperty(ref _modeloArticulo, value);
            }
        //Declarar tipo navegacion
        public Tipoarticulo tipoNavigationSelecionado
            {
                get => _tipoNavigationSelecionado;
            set => SetProperty(ref _tipoNavigationSelecionado, value);
        }


        #endregion
        // Aquí puedes añadir propiedades y métodos específicos para el ViewModel de Artículo
        public VMModeloArticulo(ModeloArticuloRepository modeloArticuloRepository,
                              TipoArticuloRepository tipoArticuloRepository)
            {
            //REPOSITORIOS 
                _modeloArticuloRepository = modeloArticuloRepository;
                _tipoArticuloRepository = tipoArticuloRepository;
                _modeloArticulo = new Modeloarticulo();

            // COLLECTION VIEW Y FILTROS
            _criterios = new List<Predicate<Modeloarticulo>>();
            InicializaCriterios();
            _predicadoFiltros = new Predicate<object>(FiltroCriterios);
        }

        //----------------
        //DIALOGO MODELO ARTICULO
        //----------------
        //Listar tipos de artículos
        public async Task InicializaTipoArticulo()
            {
                try
                {
                    _listaTipoArticulos = await GetAllAsync<Tipoarticulo>(_tipoArticuloRepository);
            }
                catch (Exception ex)
                {
                    MensajeError.Mostrar("GESTIÓN ARTÍCULOS", "Error al cargar los tipos de articulo\n" +
                        "No puedo conectar con la base de datos", 0);
                }
            }

        //Listar modelo articulos
        public async Task InicializarModelosArticulos()
            {
                try
                {
                    _listaModelosArticulos = await GetAllAsync<Modeloarticulo>(_modeloArticuloRepository);
                    OnPropertyChanged(nameof(listaModelos));

                listaModelo_CollectionView = new ListCollectionView(_listaModelosArticulos); //Inicializar lista collection view
            }
                catch (Exception ex)
                {
                    MensajeError.Mostrar("GESTIÓN MODELOS ARTÍCULOS", "Error al cargar los modelos de articulo\n" +
                        "No puedo conectar con la base de datos", 0);
                }
        }

        public async Task<bool> GuardarModeloArticuloAsync()
            {
                bool correcto = true;
                try
                {
                    if (modeloArticulo.Idmodeloarticulo == 0)
                    {
                        // Nuevo modelo de artículo
                        await _modeloArticuloRepository.AddAsync(modeloArticulo);
                    }
                    else
                    {
                        // Actualizar modelo de artículo existente
                        await _modeloArticuloRepository.UpdateAsync(modeloArticulo);
                    }
                }
                catch (Exception ex)
                {
                    // Capturamos la excepción y la registramos en el log
                    correcto = false;
                }
                return correcto;
            }

        #region Metodos privados de filtrado
        // InicializaCriterios
        private void InicializaCriterios()
        {
            _criterioTipoNavigation = new Predicate<Modeloarticulo>(
                m => m.TipoNavigation != null && m.TipoNavigation.Equals(tipoNavigationSelecionado)
            );
        }

        private void AddCriterios()
        {
            _criterios.Clear();
            if (_criterioTipoNavigation != null)
            {
                _criterios.Add(_criterioTipoNavigation);
            }
        }

        private bool FiltroCriterios(object item)
        {
            bool correcto = true;
            Modeloarticulo modeloArticulo = (Modeloarticulo)item;
            if (_criterios != null)
            {
                correcto = _criterios.TrueForAll(x => x(modeloArticulo));
            }
            return correcto;
        }

        public void Filtrar()
        {
            AddCriterios();
            listaModelo_CollectionView.Filter = _predicadoFiltros;
        }

        public void LimpiarFiltros()
        {
            tipoNavigationSelecionado = null;
            listaModelo_CollectionView.Filter = null;
        }

        #endregion
    }
}
