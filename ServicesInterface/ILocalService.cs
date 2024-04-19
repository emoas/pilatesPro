using Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
    public interface ILocalService
    {
        IEnumerable<LocalDTO> GetAll();
        LocalDTO Add(LocalDTO localDTO);
        LocalDTO Update(LocalDTO localDTOUpdate);
        void Remove(int localId);
    }
}
