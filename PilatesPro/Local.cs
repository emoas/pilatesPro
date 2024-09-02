using System;
using System.Collections.Generic;

namespace Domain
{
    public class Local
    {
        public int Id { get; set; }
        private string nombre;
        public string Direccion { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Url { get; set; }
        public bool Activo { get; set; }
        public ICollection<Profesor> Profesores { get; set; }
        public ICollection<Actividad> Actividades { get; set; }
        public DateTime FechaAlta { get; set; }
        public ICollection<Agenda> Agendas { get; set; }

        public Local()
        {
            this.FechaAlta = DateTime.Now.Date;
            this.Activo = true;
        }
        public Local(string nombre, string direccion, string pais, string ciudad, string email, string telefono, string celular, string url)
        {
            this.Nombre = nombre;
            this.Direccion = direccion;
            this.Pais = pais;
            this.Ciudad = ciudad;
            this.Email = email;
            this.Telefono = telefono;
            this.Celular = celular;
            this.Url = url;
            this.FechaAlta = DateTime.Now.Date;
            this.Activo = true;
        }
        public string Nombre
        {
            get
            {
                return this.nombre;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("El nombre del local no puede ser vacío o nulo.");
                }
                this.nombre = value;
            }
        }
    }
}
