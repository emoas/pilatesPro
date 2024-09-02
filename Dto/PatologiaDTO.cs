using System;
using System.Collections.Generic;
using System.Text;

namespace Dto
{
    public class PatologiaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public ICollection<AlumnoDTO> Alumnos { get; set; }
        public ICollection<ProfesorDTO> Profesores { get; set; }
        public PatologiaDTO()
        {
        }
    }
}
