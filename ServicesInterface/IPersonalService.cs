using Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
    public interface IPersonalService
    {
        IEnumerable<PersonalDTO> GetAll();
        PersonalDTO Add(PersonalDTO personalDTO);
        AlumnoDTO Update(PersonalDTO personalDTOUpdate);
        void Remove(int personalId);
    }
}
