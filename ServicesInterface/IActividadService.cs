using Dto;
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
    }
}
