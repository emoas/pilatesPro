using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dto
{
    public class ProfesorDTO:UserDTO
    {
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Direccion { get; set; }
        public string Celular { get; set; }
        public string Ciudad { get; set; }
        public string EmeregenciaMovil { get; set; }
        public string ContactoEmergencia { get; set; }
        public string TelefonoContacto { get; set; }
        public ICollection<PatologiaDTO> PatologíasQuePresenta { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaAlta { get; set; }
        public ICollection<ActividadDTO> Actividades { get; set; }
        public ICollection<LocalDTO> Locales { get; set; }
        public ProfesorDTO()
        {
        }
    }
}
