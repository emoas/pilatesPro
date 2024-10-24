using Domain.Alumnos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Alumno : User
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
        public ICollection<Patologia> PatologíasQuePresenta { get; set; }
        public string Observaciones { get; set; }
        public bool Activo { get; set; }
        //public enum estado { NORMAL, MANTENIMIENTO, SUSPENDIDO }
        //public estado Estado { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaAlta { get; set; }
        public int?  PlanId { get; set; }
        public Plan? Plan { get; set; }
        public ICollection<ClaseFija> ClasesFijas { get; set; }
        public ICollection<AlumnoClase> ClasesAlumno { get; set; }
        public int CuposPendientes { get; set; }
        public ICollection<Falta> Faltas { get; set; } = new List<Falta>();
        public ICollection<CupoPendiente> Cupos { get; set; } = new List<CupoPendiente>();

        public Alumno()
        {
            base.Rol = rol.ALUMNO;
            this.FechaAlta = DateTime.Now.Date;
            this.PatologíasQuePresenta = new List<Patologia>();
            this.Password = "primera";
            this.Token = Guid.NewGuid();
            this.CuposPendientes = 0;
        }
        public Alumno(string name, List<Patologia> patologias)
        {
            base.Name = name;
            this.FechaAlta = DateTime.Now.Date;
            this.PatologíasQuePresenta = patologias;
        }
    }
}
