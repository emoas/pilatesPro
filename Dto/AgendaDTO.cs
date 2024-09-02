using System;
using System.Collections.Generic;
using System.Text;

namespace Dto
{
    public class AgendaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Dia { get; set; }
        public string Hora { get; set; }
        public DateTime HorarioInicio { get; set; }
        public DateTime HorarioFin { get; set; }
        public string Color { get; set; }
        public int? ClaseId { get; set; }
        public ClaseDTO? Clase { get; set; }
        public int? LocalId { get; set; }
        public LocalDTO? Local { get; set; } = null!;
        public AgendaDTO()
        {
        }
    }
}
