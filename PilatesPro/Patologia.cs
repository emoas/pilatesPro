using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Patologia
    {
        public int Id { get; set; }
        private string nombre;
        public bool Activo { get; set; }
        public ICollection<Alumno> Alumnos { get; set; }
        public ICollection<Profesor> Profesores { get; set; }
        public Patologia()
        {
        }
        public Patologia(string nombre)
        {
            Nombre = nombre;
            Activo = true;
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
                    throw new ArgumentException("El nombre de la patologia no puede ser vacío o nulo.");
                }
                this.nombre = value;
            }
        }
    }
}
