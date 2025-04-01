using System;
using System.Collections.Generic;

namespace Domain
{
    public class Profesor:User
    {
        public string Apellido { get; set; }
        public string Sobrenombre { get; set; }
        public string Cedula { get; set; }
        public string Direccion { get; set; }
        public string Celular { get; set; }
        public string Ciudad { get; set; }
        public string EmeregenciaMovil { get; set; }
        public string ContactoEmergencia { get; set; }
        public string TelefonoContacto { get; set; }
        public ICollection<Patologia> PatologíasQuePresenta { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaAlta { get; set; }
        public ICollection<Actividad> Actividades { get; set; }
        public ICollection<Local> Locales { get; set; }

        public Profesor()
        {
            base.Rol = rol.PROFE;
            this.FechaAlta = DateTime.Now.Date;
            this.Actividades = new List<Actividad>();
            this.Locales = new List<Local>();
            this.PatologíasQuePresenta = new List<Patologia>();
            this.Password = "primera";
            this.Token = Guid.NewGuid();
        }
        public Profesor(string name, string apellido, string sobrenombre, string email, string cedula,string direccion, string celular,string ciudad, string movilEmergencia,string contactoEmergencia,string telefonoContacto,List<Patologia> patologias, bool activo, DateTime fechaNacimiento, List<Actividad> actividades, List<Local> locales)
        {
            base.Name = name;
            this.Apellido = apellido;
            this.Sobrenombre = Sobrenombre;
            this.Email = email;
            this.Password = "primera";
            this.Token = Guid.NewGuid();
            this.Cedula = cedula;
            this.Direccion = direccion;
            this.Celular = celular;
            this.Ciudad = ciudad;
            this.EmeregenciaMovil = movilEmergencia;
            this.ContactoEmergencia = contactoEmergencia;
            this.TelefonoContacto = telefonoContacto;
            this.PatologíasQuePresenta = patologias;
            this.Activo = activo;
            this.FechaNacimiento = fechaNacimiento;
            base.Rol = rol.PROFE;
            this.FechaAlta = DateTime.Now.Date;
            this.Actividades = actividades;
            this.Locales = locales;
        }

    }
}
