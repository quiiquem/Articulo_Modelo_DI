using articulomodelo.Backend.Modelo;
using articulomodelo.Backend.Servicios;
using articulomodelo.MVVM.Implementacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace articulomodelo.MVVM
{
    public class VMDepartamento : MVBase
    {

        private Departamento _departamento;
        private DepartamentoRepository _departamentoRepository;
        private List<Departamento> _listaDepartamentos;

        public VMDepartamento(DepartamentoRepository departamentoRepository)
        {
            _departamentoRepository = departamentoRepository;
            _departamento = new Departamento();
            _listaDepartamentos = new List<Departamento>();
        }

        public Departamento Departamento
        {
            get => _departamento;
            set => SetProperty(ref _departamento, value);
        }

        public List<Departamento> listaDepartamentos
        {
            get => _listaDepartamentos;
            set => SetProperty(ref _listaDepartamentos, value);
        }

        public async Task InicializarDepartamentos()
        {
            try{
                listaDepartamentos = await _departamentoRepository.GetAllConUsuariosySalidasAsync();
            } catch (Exception e)
            {

            }
        }
    }
}
