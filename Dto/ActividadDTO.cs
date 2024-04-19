using System;
using System.Collections.Generic;
using System.Text;

namespace Dto
{
    public class ActividadDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }
        public ActividadDTO()
        {
        }
    }
}
