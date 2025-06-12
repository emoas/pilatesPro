using Domain.Alumnos;
using Dto;
using Dto.Alumnos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        void CancelReservaManual(int idAlumnoClase, bool addFalta);
        void DeleteCancelAlumnoClase(int idAlumnoClase, bool ajustarEliminadas);
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
        void AgregarFalta(int alumnoId, int claseId, AlumnoClase.estado estado = AlumnoClase.estado.FALTA);
        void QuitarFalta(int idAlumnoClase);
        int ObtenerFaltasDelMes(int alumnoId, DateTime fecha);
        int CuposPendientes(int alumnoId);
        int CountCancelaciones(int alumnoId, DateTime fecha);
        IEnumerable<CupoPendienteDTO> CuposRecuperacion(int alumnoId);
        void AgregarLicencia(LicenciaAlumnoDTO licenciaAlumno);
        void EliminarLicencia(int idLicencia);
        AlumnoDTO GetLicenciaAlumno(int alumnoId);
        bool EstaDeLicencia(int idAlumno, DateTime fecha);
        Task<List<string>> DeshabilitarUsuariosInactivosAsync();
    }
}
