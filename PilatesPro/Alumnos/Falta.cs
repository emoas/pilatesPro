using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Alumnos
{
    public class Falta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int AlumnoId { get; set; }    
        public Alumno Alumno { get; set; }
        public int ClaseId { get; set; }
        public Clase Clase { get; set; }
    }
}
