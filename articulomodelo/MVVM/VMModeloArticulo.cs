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
        private Predicate<Modeloarticulo> _criterioNombreModelo;

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

        //Inicializar objeto modelo articulo para editar
        public async Task Inicializa()
        {
            try
            {
                // Cargar tipos de artículos
                _listaTipoArticulos = await GetAllAsync<Tipoarticulo>(_tipoArticuloRepository);
                OnPropertyChanged(nameof(listaTiposArticulos));

                // Cargar modelos de artículos
                _listaModelosArticulos = await GetAllAsync<Modeloarticulo>(_modeloArticuloRepository);
                listaModelo_CollectionView = new ListCollectionView(_listaModelosArticulos);
                OnPropertyChanged(nameof(listaModelo_CollectionView));
                OnPropertyChanged(nameof(listaModelos));
            }
            catch (Exception ex)
            {
                MensajeError.Mostrar("GESTIÓN MODELO ARTICULO",
                    "Error al cargar los datos\nNo puedo conectar con la base de datos", 0);
            }
        }

        public async Task<bool> GuardarModeloArticuloAsync()
        {
            bool correcto;

            if (modeloArticulo.Idmodeloarticulo == 0)
            {
                // Nuevo modelo de artículo
                correcto = await AddAsync(_modeloArticuloRepository, modeloArticulo);
            }
            else
            {
                // Actualizar modelo de artículo existente
                correcto = await UpdateAsync(_modeloArticuloRepository, modeloArticulo);
            }

            if (correcto)
            {
                // Recargar la lista completa desde BD
                await InicializarModelosArticulos();

                // Refrescar la CollectionView
                listaModelo_CollectionView?.Refresh();
            }
            else
            {
                MensajeError.Mostrar("GUARDAR MODELO",
                    "Error al guardar el modelo de artículo", 0);
            }

            return correcto;
        }



        //Eliminar modelo articulo de la BD

        public async Task<bool> EliminarModeloArticuloAsync(int id)
        {
            bool correcto = await DeleteAsync(_modeloArticuloRepository, id);

            if (!correcto)
            {
                MensajeError.Mostrar("ELIMINAR MODELO",
                    "Error al eliminar el modelo de artículo", 0);
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

            _criterioNombreModelo = new Predicate<Modeloarticulo>(m =>
                    (!string.IsNullOrEmpty(m.Nombre) && m.Nombre.ToLower().StartsWith(NombreModeloFiltro.ToLower())));
                     
        }


        //Declarar Filtro nombre de modelo filtro
        private string? _NombreModeloFiltro;

        public string? NombreModeloFiltro
        {
            get => _NombreModeloFiltro;

            set
            {
                SetProperty(ref _NombreModeloFiltro, value);
            }
        }

        private void AddCriterios()
        {
            _criterios.Clear();
            if (_criterioTipoNavigation != null)
            {
                _criterios.Add(_criterioTipoNavigation);
            }

            if(_criterioNombreModelo != null)
            {
                _criterios.Add(_criterioNombreModelo);
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
            InicializaCriterios();
            AddCriterios();
            listaModelo_CollectionView.Filter = _predicadoFiltros;
        }

        public void LimpiarFiltros()
        {
            tipoNavigationSelecionado = null;
            listaModelo_CollectionView.Filter = null;
            listaModelo_CollectionView.Refresh();
        }

        #endregion
    }
}
