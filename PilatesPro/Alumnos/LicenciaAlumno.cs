using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Alumnos
{
    public class LicenciaAlumno
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public Alumno Alumno { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public enum tipo { Anual, Medica, Otra }
        public tipo Tipo { get; set; }
        public int CantidadDias { get; set; }
        public string Observaciones { get; set; }
    }
}
