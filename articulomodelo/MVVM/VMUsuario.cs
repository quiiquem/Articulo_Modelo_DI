using articulomodelo.Backend.Modelo;
using articulomodelo.Backend.Servicios;
using articulomodelo.Frontend.Mensajes;
using articulomodelo.MVVM.Implementacion;
using ProyectoDI_Trimestre1.Frontend.Mensajes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace articulomodelo.MVVM
{
    public class VMUsuario : MVBase, IDataErrorInfo
    {

        #region campos y propiedades privadas
        //Usuario
        private Usuario _usuario;
        //Repositorio para gestionar las operaciones de datos relacionadas con los usuarios
        private UsuarioRepository _usuarioRepository;
        private DepartamentoRepository _departamentoRepository;
        private RolRepository _rolRepository;
        private GrupoRepository _grupoRepository;
        private TipoUsuarioRepository _tipoUsuarioRepository;

        private List<Rol> _listaRol;
        private List<Grupo> _listaGrupo;
        private List<Departamento> _listaDepartamentos;
        private List<Tipousuario> _listaTipoUsuario;
        private List<Usuario> _listaUsuario;

        #endregion
        #region Getters y Setters
        public List<Rol> listaRol => _listaRol;
        public List<Grupo> listaGrupo => _listaGrupo;

        public List<Usuario> listaUsuario => _listaUsuario;
        public List<Departamento> listaDepartamentos => _listaDepartamentos;

        public List<Tipousuario> listaTipoUsuario => _listaTipoUsuario;
        public Usuario usuario
        {
            get => _usuario;
            set => SetProperty(ref _usuario, value);
        }
        #endregion
        public VMUsuario(UsuarioRepository usuarioRepository,
                            DepartamentoRepository departamentoRepository,
                            RolRepository rolRepository,
                            GrupoRepository grupoRepository,
                            TipoUsuarioRepository tipoUsuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _departamentoRepository = departamentoRepository;
            _rolRepository = rolRepository;
            _grupoRepository = grupoRepository;
            _tipoUsuarioRepository = tipoUsuarioRepository;
            _usuario = new Usuario();
        }


        //-----------------
        //CONTROL DE USUARIO

   

        //Listar usuarios (usuario control)
        public async Task InicializarUsuarios()
        {
            try
            {
                _listaUsuario = await _usuarioRepository.GetAllWithRelationsAsync(); 
                OnPropertyChanged(nameof(listaUsuario));
            }
            catch (Exception ex)
            {
                MensajeError.Mostrar("GESTIÓN USUARIOS", "Error al cargar los usuarios\n" +
                    "No puedo conectar con la base de datos", 0);
            }
        }

        //-----------------
        //DIALOGO ARTICULO
        //-----------------

        //Listar departamentos
        public async Task InicializarDepartamentos()
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

        //Listar roles
        public async Task InicializarRoles()
        {
            try
            {
                _listaRol = await GetAllAsync<Rol>(_rolRepository);
                OnPropertyChanged(nameof(listaRol));
            }
            catch (Exception ex)
            {
                MensajeError.Mostrar("GESTIÓN ROLES", "Error al cargar los roles\n" +
                    "No puedo conectar con la base de datos", 0);
            }
        }

        //Listar grupos
        public async Task InicializarGrupos()
        {
            try
            {
                _listaGrupo = await GetAllAsync<Grupo>(_grupoRepository);
                OnPropertyChanged(nameof(listaGrupo));
            }
            catch (Exception ex)
            {
                MensajeError.Mostrar("GESTIÓN GRUPOS", "Error al cargar los grupos\n" +
                    "No puedo conectar con la base de datos", 0);
            }
        }

        //Listar tipos de usuario
        public async Task InciializarTipoUsuario()
        {
            try
            {
                _listaTipoUsuario = await GetAllAsync<Tipousuario>(_tipoUsuarioRepository);
                OnPropertyChanged(nameof(listaTipoUsuario));
            }
            catch (Exception ex)
            {
                MensajeError.Mostrar("GESTIÓN TIPO USUARIOS", "Error al cargar los tipos\n" +
                    "No puedo conectar con la base de datos", 0);
            }
        }



        //GUARDAR USUARIO

        public async Task<bool> GuardarUsuarioAsync()
        {
            bool correcto = true;
            try
            {
                if (usuario.Idusuario == 0)
                {
                    // Nuevo usuario
                    await _usuarioRepository.AddAsync(usuario);
                }
                else
                {
                    // Actualizar usuario existente
                    await _usuarioRepository.UpdateAsync(usuario);
                }
            }
            catch (Exception ex)
            {
                // Capturamos la excepción y la registramos en el log
                correcto = false;
            }
            return correcto;
        }



    }
}
