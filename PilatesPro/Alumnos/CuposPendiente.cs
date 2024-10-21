using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Alumnos
{
    public class CupoPendiente
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public Alumno Alumno { get; set; }  
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public enum tipo {CANCELACIONADMIN, CANCELACIONWEB, ADMINISTRACION }
        public tipo Tipo { get; set; }
        public enum estado { PENDIENTE, EXPIRADO, UTILIZADO }
        public estado Estado { get; set; }

    }
}
