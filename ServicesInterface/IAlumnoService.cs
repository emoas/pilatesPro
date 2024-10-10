using Domain.Alumnos;
using Dto;
using Dto.Alumnos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServicesInterface
{
    public interface IAlumnoService
    {
        IEnumerable<AlumnoDTO> GetAll();
        AlumnoDTO Add(AlumnoDTO alumnoDTO);
        void AddAlumnoClase(AlumnoClaseDTO alumnoClase);
        AlumnoDTO Update(AlumnoDTO alumnoDTOUpdate);
        void Desactivate(int alumnoId);
        void RemoveAlumnoClase(int alumnoId, int claseId);
        void CancelReservaManual(int alumnoId, int claseId);
        void CancelReservaWeb(int alumnoId, int claseId);
        void addClasesFijasAlumno(int alumnoId, ClaseFijaDTO claseFija);
        void RemoveClaseFija(int id);
        AlumnoDTO GetId(int alumnoId);
        ClaseFijaDTO AddClaseFija(int idAlumno, ClaseFijaDTO claseFija);
        ClaseFijaDTO UpdateClaseFija(int id, ClaseFijaDTO claseFija);
        IEnumerable<ClaseFijaDTO> GetFijasAlumno(int idAlumno);
        IEnumerable<AlumnoClaseDTO> GetMisReservas(int idAlumno);
        int GetMisReservasSemana(int alumnoId, DateTime dayOfWeek);
        IEnumerable<AlumnoClaseDTO> GetReservasPeriodo(int alumnoId, DateTime desde, DateTime hasta);
        bool agregarAlumnoAClase(int alumnoId, int claseId, AlumnoClase.tipo tipo);
        void UpdateClasesAlumno(int claseId);
        void AgregarFalta(int alumnoId, int claseId);
        int ObtenerFaltasDelMes(int alumnoId, DateTime fecha);
    }
}
