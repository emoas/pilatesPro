using Dto.Alumnos;
using System;
using System.Collections.Generic;

namespace Dto
{
    public class AlumnoDTO:UserDTO
    {
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Ciudad { get; set; }
        public string EmeregenciaMovil { get; set; }
        public string ContactoEmergencia { get; set; }
        public string TelefonoContacto { get; set; }
        public ICollection<PatologiaDTO> PatologíasQuePresenta { get; set; }
        public string Observaciones { get; set; }
        public bool Activo { get; set; }
        //public enum estado { NORMAL, MANTENIMIENTO, SUSPENDIDO }
        //public estado Estado { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaAlta { get; set; }
        public int? PlanId { get; set; }
        public PlanDTO? Plan { get; set; }
        public ICollection<ClaseFijaDTO> ClasesFijas { get; set; }
        public ICollection<ClaseDTO> Clases { get; set; }
        public int CuposPendientes { get; set; }
        public ICollection<LicenciaAlumnoDTO> Licencia { get; set; }
        public AlumnoDTO()
        {
        }
    }
}
