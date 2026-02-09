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
        private List<Grupo> _listaGrupo;

        public VMGrupo(GrupoRepository grupoRepository)
        {
            _grupoRepository = grupoRepository;
            _grupo = new Grupo();
            _listaGrupo = new List<Grupo>();
        }

        public Grupo grupo
        {
            get => _grupo;
            set => SetProperty(ref _grupo, value);
        }
        public List<Grupo> listaGrupo
        {
            get => _listaGrupo;
            set => SetProperty(ref _listaGrupo, value);
        }

        public async Task InicializarGrupos()
        {
            try
            {
                listaGrupo = await _grupoRepository.GetAllAsync();
            }
            catch (Exception e)
            {
                MensajeError.Mostrar("GESTIÓN GRUPOS", "Error al cargar los grupos\n" +
                "No puedo conectar con la base de datos", 0);
            }
        }
    }
}
