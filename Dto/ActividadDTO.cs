using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dto
{
    public class ActividadDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string DescripcionCorta { get; set; }
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }
        public ICollection<ProfesorDTO> Profesores { get; set; }
        public ICollection<LocalDTO> Locales { get; set; }
        public ICollection<ClaseDTO> Clases { get; set; }
        public ICollection<PlanDTO> Planes { get; set; }
        public ActividadDTO()
        {
        }
    }
}
