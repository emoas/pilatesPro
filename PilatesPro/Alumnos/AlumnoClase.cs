using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Alumnos
{
    public class AlumnoClase
    {
        public int Id { get; set; }
        public int AlumnoId { get; set; }
        public Alumno Alumno { get; set; }
        public enum tipo { FIJO, PUNTUAL, WEB, RECUPERACION, ADMIN }
        public tipo Tipo { get; set; }
        public enum estado { CONFIRMADA, PENDIENTE, CANCELADA }
        public estado Estado { get; set; }
        public int ClaseId { get; set; }
        public Clase Clase { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaCancelacion { get; set; } // Para saber cuándo fue cancelada
        public string Codigo { get; private set; }
        public bool Asistio { get; set; }

        // Constructor para inicializar el código automáticamente
        public AlumnoClase()
        {
            Codigo = GenerarCodigoReserva();
            Asistio = true;
        }

        private string GenerarCodigoReserva()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] codigo = new char[8];

            for (int i = 0; i < codigo.Length; i++)
            {
                codigo[i] = caracteres[random.Next(caracteres.Length)];
            }

            return new string(codigo);
        }
    }
}
