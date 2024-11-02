using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dto.Alumnos
{
    public class AlumnoClaseDTO
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public AlumnoDTO Alumno { get; set; }
        public enum tipo { FIJO, PUNTUAL, WEB, RECUPERACION, ADMIN }
        public tipo Tipo { get; set; }
        public enum estado { CONFIRMADA, PENDIENTE, CANCELADA }
        public estado Estado { get; set; }
        public int ClaseId { get; set; }
        public ClaseDTO Clase { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaCancelacion { get; set; } // Para saber cuándo fue cancelada
    }
}
