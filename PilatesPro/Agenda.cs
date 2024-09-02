using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Agenda
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Dia { get; set; }
        public string Hora { get; set; }
        public DateTime HorarioInicio { get; set; }
        public DateTime HorarioFin { get; set; }
        public string Color { get; set; }
        public int? ClaseId { get; set; } 
        public Clase? Clase { get; set; }
        public int? LocalId { get; set; }
        public Local? Local { get; set; }
        public Agenda()
        {
        }
    }
}
