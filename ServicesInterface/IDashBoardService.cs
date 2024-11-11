using Dto;
using Dto.DashBoard;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
    public interface IDashBoardService
    {
        DashBoardDTO GetHome();
        IEnumerable<AgendaDTO> GetClasesLocalFecha(int idLocal, DateTime fecha);
        IEnumerable<ClaseDTO> GetClasesProfeFecha(int idProfe, DateTime fecha);
    }
}
