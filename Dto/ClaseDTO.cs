using Dto.Alumnos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dto
{
   public class ClaseDTO
    {
        public int Id { get; set; }
        public LocalDTO Local { get; set; }
        public ProfesorDTO Profesor { get; set; }
        public bool Activo { get; set; }
        public int ActividadId { get; set; }
        public ActividadDTO Actividad { get; set; }
        public string? ActividadNombre { get; set; }
        public AgendaDTO? Agenda { get; set; }
        public DateTime HorarioInicio { get; set; }
        public DateTime HorarioFin { get; set; }
        public int CuposTotales { get; set; }
        public int CuposOtorgados { get; set; }
        public int CuposConfirmados { get; set; } // importante
        public ICollection<AlumnoClaseDTO> ClasesAlumno { get; set; }
        public ClaseDTO()
        {
        }
    }
}
