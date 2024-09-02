using System;
using System.Collections.Generic;
using System.Text;

namespace Dto
{
    public class HorarioPuntualDTO
    {
        public int Id { get; set; }
        public DateTime HorarioInicio { get; set; }
        public DateTime HorarioFin { get; set; }
        public int CuposTotales { get; set; }
        public int CuposOtorgados { get; set; }
        public ICollection<AlumnoDTO> Alumnos { get; set; }

        public HorarioPuntualDTO()
        {
        }
    }
    public class HorarioVariableDTO
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public ICollection<DiaConHorasVariablesDTO> Dias { get; set; }
    }
    public class DiaConHorasVariablesDTO
    {
        public int Id { get; set; }
        public string NombreDia { get; set; }
        public List<ItemsDTO> Horas { get; set; }

        public DiaConHorasVariablesDTO(string nombreDia)
        {
            NombreDia = nombreDia;
            Horas = new List<ItemsDTO>();
        }

        public void AgregarItem(ItemsDTO item)
        {
            Horas.Add(item);
        }
    }
    public class ItemsDTO
    {
        public int Id { get; set; }
        public string Hora { get; set; }
        public int CuposTotales { get; set; }
        public int CuposOtorgados { get; set; }
        public ICollection<AlumnoDTO> Alumnos { get; set; }

    }
}

