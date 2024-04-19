using Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
    public interface IAlumnoService
    {
        IEnumerable<AlumnoDTO> GetAll();
        AlumnoDTO Add(AlumnoDTO alumnoDTO);
        AlumnoDTO Update(AlumnoDTO alumnoDTOUpdate);
        void Remove(int alumnoId);
    }
}
