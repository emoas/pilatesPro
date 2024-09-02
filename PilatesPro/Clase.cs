using Domain.Alumnos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Clase
    {
        public int Id { get; set; }
        public Local Local { get; set; }
        public Profesor Profesor { get; set; }
        public bool Activo { get; set; }
        public int ActividadId { get; set; }
        public Actividad Actividad { get; set; }
        public Agenda? Agenda { get; set; }
        public DateTime HorarioInicio { get; set; }
        public DateTime HorarioFin { get; set; }
        public int CuposTotales { get; set; }
        public int CuposOtorgados { get; set; }
        public ICollection<AlumnoClase> ClasesAlumno { get; set; }

        public Clase()
        {
            this.Activo = true;
            this.CuposOtorgados = 0;
            this.ClasesAlumno = new List<AlumnoClase>();
        }
        public Clase(Local local, Profesor profesor,int actividadId, DateTime horarioInicio, DateTime horarioFin, int cuposTotales, int cuposOtorgados)
        {
            this.Local = local;
            this.Profesor = profesor;
            this.ActividadId = actividadId;
            this.Activo = true;
            this.HorarioFin = horarioFin;
            this.HorarioInicio = horarioInicio;
            this.CuposOtorgados = cuposOtorgados;
            this.CuposTotales = cuposTotales;
        }
    }
}
