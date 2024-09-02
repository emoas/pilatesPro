using Dto;
using Dto.Profesores;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
    public interface IActividadService
    {
            IEnumerable<ActividadDTO> GetAll();
            ActividadDTO Add(ActividadDTO actividadDTO);
            ActividadDTO Update(ActividadDTO actividadDTOUpdate);
            void Remove(int actividadId);
            ActividadDTO GetId(int actividadId);
            IEnumerable<ProfesorLightDTO> GetProfesores(int actividadId);
            ActividadDTO Activar(int actividadId, bool activar);
            IEnumerable<ActividadDTO> GetPorLocal(int localId);
    }
}
