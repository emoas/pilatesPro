using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Alumnos
{
    public class AlumnoClase
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public Alumno Alumno { get; set; }
        public enum tipo { FIJO,PUNTUAL,WEB}
        public tipo Tipo { get; set; }
        public int ClaseId { get; set; }
        public Clase Clase { get; set; }
    }
}
