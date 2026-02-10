using articulomodelo.Backend.Modelo;
using articulomodelo.Backend.Servicios;
using articulomodelo.Frontend.Mensajes;
using articulomodelo.MVVM.Implementacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace articulomodelo.MVVM
{
    public class VMGrupo : MVBase
    {
        private Grupo _grupo;
        private GrupoRepository _grupoRepository;
        private List<Grupo> _listaGrupos;

        public VMGrupo(GrupoRepository grupoRepository)
        {
            _grupoRepository = grupoRepository;
            _grupo = new Grupo();
            _listaGrupos = new List<Grupo>();
        }

        public Grupo grupo
        {
            get => _grupo;
            set => SetProperty(ref _grupo, value);
        }
        public List<Grupo> listaGrupos
        {
            get => _listaGrupos;
            set => SetProperty(ref _listaGrupos, value);
        }


        //Si en el examen hubiera q usar tree en el mismo de crear prob deberia separar metodos 
        // para cargar los grupos con sus usuarios y salidas y otro para cargar solo los grupos sin relaciones
        public async Task InicializarGrupos_Arbol()
        {
            try
            {
                listaGrupos = await _grupoRepository.GetAllConUsuariosySalidas();
            }
            catch (Exception e)
            {
                MensajeError.Mostrar("GESTIÓN GRUPOS", "Error al cargar los grupos\n" +
                "No puedo conectar con la base de datos", 0);
            }
        }
    }
}
