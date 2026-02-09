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

        public VMEspacio(EspacioRepository espacioRepository)
        {
            _espacioRepository = espacioRepository;
            _espacio = new Espacio();
            _listaEspacios = new List<Espacio>();
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

        public async Task InicializarEspacios()
        {
            try
            {
                listaEspacios = await _espacioRepository.GetAllConArticulosAsync();
            }
            catch (Exception ex)
            {
                MensajeError.Mostrar("GESTIÓN ESPACIOS", "Error al cargar los espacios\n" +
                    "No puedo conectar con la base de datos", 0);
            }
        }
    }
}