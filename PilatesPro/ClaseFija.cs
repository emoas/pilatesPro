using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ClaseFija
    {
        public int Id { get; set; }
        public int ActividadId { get; set; }
        public Actividad Actividad { get; set; }
        public int LocalId { get; set; }
        public string Dia { get; set; }
        public string Hora { get; set; }
        public int AlumnoId { get; set; }
        public Alumno Alumno { get; set; }
    }
}
