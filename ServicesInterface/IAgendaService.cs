using Domain;
using Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
    public interface IAgendaService
    {
        IEnumerable<AgendaDTO> GetAll();
        IEnumerable<AgendaDTO> GetPorFecha(int localId,DateTime fecha);
        IEnumerable<AgendaDTO> GetLocalId(int localId);
        AgendaDTO Add(AgendaDTO agendaDTO);
        AgendaDTO Update(AgendaDTO agendaDTOUpdate);
        void Remove(int agendaId);
        List<Clase> GetClasesFijas(int localId, int actividadId, string dia, string hora);
    }
}
