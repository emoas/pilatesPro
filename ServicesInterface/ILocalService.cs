using Domain;
using Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServicesInterface
{
    public interface ILocalService
    {
        IEnumerable<LocalDTO> GetAll();
        LocalDTO Add(LocalDTO localDTO);
        Task SendMessageAsync(WhatsAppMessage message);
        LocalDTO Update(LocalDTO localDTOUpdate);
        void Remove(int localId);
        LocalDTO GetId(int localId);
    }
}
