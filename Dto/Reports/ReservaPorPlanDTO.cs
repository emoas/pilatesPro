using System;
using System.Collections.Generic;
using System.Text;

namespace Dto.Reports
{
    public class ReservaPorPlanDTO
    {
        public int AlumnoId { get; set; }
        public string AlumnoNombre { get; set; }
        public string AlumnoApellido { get; set; }
        public int ClaseId { get; set; }
        public string ClaseNombre { get; set; }
        public string ActividadNombre { get; set; }
        public string LocalNombre { get; set; }
        public DateTime HorarioInicio { get; set; }
        public DateTime HorarioFin { get; set; }

        public ReservaPorPlanDTO()
        {
        }
    }
}
