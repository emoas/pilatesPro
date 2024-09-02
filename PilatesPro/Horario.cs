using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class HorarioPuntual
    {
        public int Id { get; set; }
        public DateTime HorarioInicio { get; set; }
        public DateTime HorarioFin { get; set; }
        public int CuposTotales { get; set; }
        public int CuposOtorgados { get; set; }
        public ICollection<Alumno> Alumnos { get; set; }
        public int ClseId { get; set; }
        public Clase Clase { get; set; }

        public HorarioPuntual()
        {
            this.CuposOtorgados = 0;
            this.Alumnos = new List<Alumno>();
        }
    }
   public class HorarioVariable
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public ICollection<DiaConHorasVariables> Dias { get; set; }

        /*// Ejemplo de uso
        DiaConHorasVariables lunes = new DiaConHorasVariables("Lunes");
        lunes.AgregarHora(8);
        lunes.AgregarHora(10);
        lunes.AgregarHora(14);

        // Accediendo a los datos
        Console.WriteLine($"Horas del {lunes.NombreDia}:");
        foreach (var hora in lunes.Horas)
        {
            Console.WriteLine(hora);
        }*/
    }
    public class DiaConHorasVariables
    {
        public int Id { get; set; }
        public string NombreDia { get; set; }
        public List<Items> Horas { get; set; }

        public DiaConHorasVariables(string nombreDia)
        {
            NombreDia = nombreDia;
            Horas = new List<Items>();
        }

        public void AgregarItem(Items item)
        {
            Horas.Add(item);
        }
    }
    public class Items
    {
        public int Id { get; set; }
        public string Hora { get; set; }
        public int CuposTotales { get; set; }
        public int CuposOtorgados { get; set; }
        public ICollection<Alumno> Alumnos { get; set; }
    }
    }
