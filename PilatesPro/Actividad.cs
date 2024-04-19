using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Actividad
    {
        public int Id { get; set; }
        private string nombre;
        public string Descripcion { get; set; }
        public DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }
        public Actividad()
        {
            FechaAlta = DateTime.Now.Date;
        }
        public Actividad(string name, string descripcion, bool activo)
        {
            Nombre = name;
            Descripcion = descripcion;
            Activo = activo;
            FechaAlta = DateTime.Now.Date;
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
                    throw new ArgumentException("El nombre del artículo no puede ser vacío o nulo.");
                }
                this.nombre = value;
            }
        }
    }
}
