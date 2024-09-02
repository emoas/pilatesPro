using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Actividad
    {
        public int Id { get; set; }
        private string nombre;
        public string DescripcionCorta { get; set; }
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }
        public ICollection<Profesor> Profesores { get; set; }
        public ICollection<Plan> Planes { get; set; }
        public ICollection<Local> Locales { get; set; }
        public ICollection<Clase> Clases { get; set; }
        public Actividad()
        {
            FechaAlta = DateTime.Now.Date;
            this.Profesores = new List<Profesor>();
            this.Locales = new List<Local>();
            this.Clases = new List<Clase>();
            this.Color = "rgb(151,0,255)";
        }
        public Actividad(string name, string descripcion, bool activo, List<Profesor> profesores, List<Local> locales, List<Clase> clases)
        {
            Nombre = name;
            Descripcion = descripcion;
            Activo = activo;
            FechaAlta = DateTime.Now.Date;
            this.Profesores = profesores;
            this.Locales = locales;
            this.Clases = clases;
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
                    throw new ArgumentException("El nombre de la actividad no puede ser vacío o nulo.");
                }
                this.nombre = value;
            }
        }
    }
}
