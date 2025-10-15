using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Plan
    {
        public int Id { get; set; }
        private string nombre;
        public enum tipo { SEMANAL, MENSUAL, ANUAL, PASE_LIBRE, TU_PASE }
        public tipo Tipo { get; set; } = tipo.SEMANAL;
        public bool Activo { get; set; }
        public string Descripcion { get; set; }
        public int Precio { get; set; }
        public int? VecesxSemana { get; set; }
        public int? VecesxMes { get; set; }
        public int? CantidadFaltas { get; set; }
        public int? CantidadCancelaciones { get; set; }
        public enum ReglaVisualizacionAgenda
        {
            PorDias,
            HastaSabadoSemanaSiguiente
        }
        public ReglaVisualizacionAgenda ReglaAgenda { get; set; } = ReglaVisualizacionAgenda.HastaSabadoSemanaSiguiente;
        public int? DiasVisualizacionAgenda { get; set; }
        public ICollection<Actividad> Actividades { get; set; }
        public int? ActividadLibreId { get; set; }
        public ICollection<Alumno> Alumnos { get; set; }
        public Plan()
        {
        }
        public Plan(string nombre)
        {
            Nombre = nombre;
            Tipo = tipo.SEMANAL;
            Activo = true;
            VecesxSemana = 0;
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
                    throw new ArgumentException("El nombre del plan no puede ser vacío o nulo.");
                }
                this.nombre = value;
            }
        }
    }
}
