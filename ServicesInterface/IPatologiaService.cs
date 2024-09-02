using Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
   public interface IPatologiaService
    {
        IEnumerable<PatologiaDTO> GetAll();
        PatologiaDTO Add(PatologiaDTO patologiaDTO);
        PatologiaDTO Update(PatologiaDTO patologiaDTO);
        void Remove(int patologiaId);
        PatologiaDTO GetId(int patologiaId);

    }
}
