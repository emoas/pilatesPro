using System;
using System.Collections.Generic;
using System.Text;

namespace Dto
{
    public class ClaseFijaDTO
    {
        public int Id { get; set; }
        public int ActividadId { get; set; }
        public ActividadDTO Actividad { get; set; }
        public int LocalId { get; set; }
        public string Dia { get; set; }
        public string Hora { get; set; }
        public int AlumnoId { get; set; }
        public AlumnoDTO Alumno { get; set; }
    }
}
