using Dto;
using Dto.Alumnos;
using Dto.Clases;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
    public interface IClaseService
    {
        IEnumerable<ClaseDTO> GetClasesActividad(int actividadId);
        ClaseDTO Add(int actividadId,ClaseDTO claseDTO);
        ClaseDTO Update(ClaseDTO claseDTOUpdate);
        void Remove(int claseId);
        ClaseDTO GetId(int claseId);
        ClasesTodayDTO GetClasesToday(DateTime today);
        IEnumerable<AlumnoClaseDTO> GetAlumnos(int claseId);
        void CopyTo(int localId,DateTime fechaDesde, DateTime fechaTo);
        IEnumerable<ClaseDTO> Between(int actividadId,DateTime fechaDesde, DateTime fechaTo);
        IEnumerable<ClaseDTO> ActividadesParaReservar(int alumnoId,int actividadId, DateTime fechaDesde, DateTime fechaTo);
        
    }
}
