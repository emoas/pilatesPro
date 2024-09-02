using System;
using System.Collections.Generic;
using System.Text;

namespace Dto
{
    public class PlanDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public enum tipo { SEMANAL, MENSUAL, ANUAL, PASE_LIBRE, TU_PASE }
        public tipo Tipo { get; set; }
        public bool Activo { get; set; }
        public string Descripcion { get; set; }
        public int Precio { get; set; }
        public int? VecesxSemana { get; set; }
        public int? VecesxMes { get; set; }
        public ICollection<AlumnoDTO> Alumnos { get; set; }
        public ICollection<ActividadDTO> Actividades { get; set; }
        public int? ActividadLibreId { get; set; }
        public PlanDTO()
        {
        }
    }
}
