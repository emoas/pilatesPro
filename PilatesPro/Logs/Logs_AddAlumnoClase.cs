using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Logs
{
    public class Logs_AddAlumnoClase
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int AlumnoId { get; set; }
        public int ClaseId { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public enum estado { PENDIENTE, RESUELTO }
        public estado Estado { get; set; }
        public enum tipo { RESERVAADMIN, CANCELACIONWEB, RESERVAWEB, CANCELACIONADMIN }
        public tipo Tipo { get; set; }
    }
}
