using System;
using System.Collections.Generic;
using System.Text;

namespace Dto.Alumnos
{
    public class CupoPendienteDTO
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public enum tipo { CANCELACIONADMIN, CANCELACIONWEB, ADMINISTRACION }
        public tipo Tipo { get; set; }
        public enum estado { PENDIENTE, EXPIRADO, UTILIZADO }
        public estado Estado { get; set; }

    }
}
