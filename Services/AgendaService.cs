using AutoMapper;
using DataAccessInterface.Repositories;
using Domain;
using Dto;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Services
{
    public class AgendaService : IAgendaService
    {
        private IRepository<Agenda> agendaRepository;
        private IMapper mapper;
        public AgendaService(IMapper mapper, IRepository<Agenda> agendaRepository)
        {
            this.agendaRepository = agendaRepository;
            this.mapper = mapper;
        }
        public AgendaDTO Add(AgendaDTO agendaDTO)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AgendaDTO> GetAll()
        {
            var agendas = this.agendaRepository.IncludeAll("Clase","Local");
            return this.mapper.Map<IEnumerable<AgendaDTO>>(agendas);
        }

        public IEnumerable<AgendaDTO> GetPorFecha(int localId,DateTime fechaDesde)
        {
            var agendas = this.agendaRepository.IncludeAll("Clase", "Local").Where(a => a.HorarioInicio.Date == fechaDesde.Date && a.LocalId==localId); ;
            return this.mapper.Map<IEnumerable<AgendaDTO>>(agendas);
        }

        public IEnumerable<AgendaDTO> GetLocalId(int localId)
        {
            // Calcular la fecha límite (hace 2 meses desde hoy)
            DateTime fechaLimite = DateTime.Now.AddMonths(-2);
            var agendas = this.agendaRepository.IncludeAll().Where(a => a.LocalId == localId && a.HorarioInicio.Date>= fechaLimite);
            return this.mapper.Map<IEnumerable<AgendaDTO>>(agendas);
        }

        public void Remove(int agendaId)
        {
            throw new NotImplementedException();
        }

        public AgendaDTO Update(AgendaDTO agendaDTOUpdate)
        {
            throw new NotImplementedException();
        }
        public List<Clase> GetClasesFijas(int localId,int actividadId,string dia, string hora)
        {
            // Obtener la fecha y hora actual
            DateTime fechaHoraActual = DateTime.Now;
            // Sumar un día
            DateTime fechaHoraMasUnDia = fechaHoraActual.AddDays(1);
            var clases = new List<Clase>();
            var agendas = this.agendaRepository.IncludeAll("Clase").Where(a => a.HorarioInicio.Date >= fechaHoraMasUnDia.Date &&  a.LocalId == localId && a.Dia==dia && a.Hora==hora);
            foreach (Agenda agenda in agendas)
            {
                if (agenda.Clase.ActividadId == actividadId)
                    clases.Add(agenda.Clase);
            }
                return clases;
        }
    }
}
