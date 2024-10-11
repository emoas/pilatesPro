using AutoMapper;
using DataAccessInterface.Repositories;
using Domain;
using Domain.Alumnos;
using Dto;
using Dto.Alumnos;
using Dto.Clases;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Services
{
    public class ClaseService : IClaseService
    {
        private IActividadRepository actividadRepository;
        private IProfeRepository profeRepository;
        private IRepository<Local> localRepository;
        private IRepository<Clase> claseRepository;
        private IRepository<AlumnoClase> alumnoClaseRepository;
        private IMapper mapper;
        private IAgendaService agendaService;
        private IAlumnoService alumnoService;
        public ClaseService(IMapper mapper, IAlumnoService alumnoService, IRepository<AlumnoClase> alumnoClaseRepository, IActividadRepository actividadRepository, IProfeRepository profeRepository, IRepository<Local> localRepository, IRepository<Clase> claseRepository, IAgendaService agendaService)
        {
            this.actividadRepository = actividadRepository;
            this.profeRepository = profeRepository;
            this.localRepository = localRepository;
            this.claseRepository = claseRepository;
            this.agendaService = agendaService;
            this.alumnoService = alumnoService;
            this.alumnoClaseRepository = alumnoClaseRepository;
            this.mapper = mapper;
        }
        public ClaseDTO Add(int actividadId, ClaseDTO claseDTO)
        {
            Clase clase = new Clase();
            Agenda agenda = new Agenda();

            var actividadToUpdate = this.actividadRepository.IncludeAll("Clases").FirstOrDefault(a => a.Id == actividadId);
            var local = this.localRepository.List().FirstOrDefault(l => l.Id == claseDTO.Local.Id);
            var profe = this.profeRepository.getProfesores().FirstOrDefault(p => p.Id == claseDTO.Profesor.Id);
            clase.Local = local;
            clase.Profesor = profe;
            //creo la agenda para la clase
            agenda.Titulo = actividadToUpdate.Nombre;
            agenda.Color = actividadToUpdate.Color;

            clase.HorarioFin = claseDTO.HorarioFin;
            clase.HorarioInicio = claseDTO.HorarioInicio;
            clase.CuposOtorgados = claseDTO.CuposOtorgados;
            clase.CuposTotales = claseDTO.CuposTotales;
            agenda.HorarioInicio = claseDTO.HorarioInicio;
            agenda.HorarioFin = claseDTO.HorarioFin;
            agenda.Local = local;
            // Configurar la cultura en español (España)
            CultureInfo culturaEspañola = new CultureInfo("es-ES");
            // Obtener el nombre del día
            agenda.Dia = claseDTO.HorarioInicio.ToString("dddd", culturaEspañola);
            // Obtener la hora y los minutos en formato de 24 horas
            agenda.Hora = claseDTO.HorarioInicio.ToString("HH:mm");
            clase.Agenda = agenda;
            actividadToUpdate.Clases.Add(clase);
            this.actividadRepository.Update(actividadToUpdate);
            return this.mapper.Map<ClaseDTO>(clase);
        }
        public IEnumerable<ClaseDTO> Between(int actividadId, DateTime fechaDesde, DateTime fechaTo)
        {
            var clases = this.claseRepository.IncludeAll("Local", "Actividad", "ClasesAlumno").Where(c => c.ActividadId == actividadId && c.Activo == true && c.HorarioInicio >= fechaDesde && c.HorarioFin <= fechaTo).OrderBy(c => c.HorarioInicio);
            return this.mapper.Map<IEnumerable<ClaseDTO>>(clases);
        }

        public IEnumerable<ClaseDTO> ActividadesParaReservar(int alumnoId,int actividadId, DateTime fechaDesde, DateTime fechaTo)
        {
            var clases = this.claseRepository.IncludeAll("Local", "Actividad", "ClasesAlumno","Profesor")
                .Where(c => c.ActividadId == actividadId
                 && c.Activo == true
                 && c.HorarioInicio >= fechaDesde
                 && c.HorarioFin <= fechaTo
                 && !c.ClasesAlumno.Any(ca => ca.AlumnoId == alumnoId)) // Excluir clases donde el alumno ya está registrado
     .OrderBy(c => c.HorarioInicio);
            return this.mapper.Map<IEnumerable<ClaseDTO>>(clases);
        }

        public void CopyTo(int localId, DateTime fechaDesde, DateTime fechaTo)
        {
            var agendas = this.agendaService.GetPorFecha(localId, fechaDesde);
            foreach (AgendaDTO agenda in agendas)
            {
                ClaseDTO claseaux = new ClaseDTO();
                var claseDTO = this.GetId((int)agenda.ClaseId);
                DateTime updatedHoarioInicio = new DateTime(fechaTo.Year, fechaTo.Month, fechaTo.Day, claseDTO.HorarioInicio.Hour, claseDTO.HorarioInicio.Minute, claseDTO.HorarioInicio.Second);
                DateTime updatedHoarioFin = new DateTime(fechaTo.Year, fechaTo.Month, fechaTo.Day, claseDTO.HorarioFin.Hour, claseDTO.HorarioFin.Minute, claseDTO.HorarioFin.Second);
                claseaux.HorarioInicio = updatedHoarioInicio;
                claseaux.HorarioFin = updatedHoarioFin;
                claseaux.CuposTotales = claseDTO.CuposTotales;
                claseaux.CuposOtorgados = claseDTO.CuposOtorgados;
                claseaux.ClasesAlumno = claseDTO.ClasesAlumno;
                claseaux.Profesor = claseDTO.Profesor;
                claseaux.Local = claseDTO.Local;
                var clase=this.Add(claseDTO.Actividad.Id, claseaux);
                this.alumnoService.UpdateClasesAlumno(clase.Id);
            }
        }

        public IEnumerable<AlumnoClaseDTO> GetAlumnos(int claseId)
        {
            var clases = this.alumnoClaseRepository.IncludeAll("Alumno").Where(ac => ac.ClaseId == claseId);
            return this.mapper.Map<IEnumerable<AlumnoClaseDTO>>(clases);
        }

        public IEnumerable<ClaseDTO> GetClasesActividad(int actividadId)
        {
            var actividadToUpdate = this.actividadRepository.IncludeAll("Clases").FirstOrDefault(a => a.Id == actividadId);
            return this.mapper.Map<IEnumerable<ClaseDTO>>(actividadToUpdate.Clases);
        }

        public ClaseDTO GetId(int claseId)
        {
            var clase = this.claseRepository.IncludeAll("ClasesAlumno", "Actividad", "Local", "Profesor").FirstOrDefault(c => c.Id == claseId);
            return this.mapper.Map<ClaseDTO>(clase);
        }

        public ClasesTodayDTO GetClasesToday(DateTime today)
        {
            var clasesDeHoy = this.claseRepository.List().Where(c => c.HorarioInicio.Date == today);
            int cantidadClasesHoy = clasesDeHoy.Count();
            int sumaCuposOtorgadosHoy = clasesDeHoy.Sum(c => c.CuposOtorgados);
            return new ClasesTodayDTO
            {
                CantidadClasesHoy = cantidadClasesHoy,
                SumaCuposOtorgadosHoy = sumaCuposOtorgadosHoy
            };
        }

        public void Remove(int claseId)
        {
            var clase = this.claseRepository.IncludeAll("Actividad", "Local", "Profesor", "Agenda", "ClasesAlumno").FirstOrDefault(c => c.Id == claseId);
            this.claseRepository.Delete(clase);
        }

        public ClaseDTO Update(ClaseDTO claseDTOUpdate)
        {
            Clase clase = this.claseRepository.IncludeAll("ClasesAlumno", "Actividad", "Local", "Profesor", "Agenda").FirstOrDefault(c => c.Id == claseDTOUpdate.Id);
            var actividad = this.actividadRepository.IncludeAll("Clases").FirstOrDefault(a => a.Id == clase.ActividadId);
            actividad.Clases.Remove(clase);
            this.actividadRepository.Update(actividad);
            claseDTOUpdate.CuposOtorgados = clase.CuposOtorgados;
            claseDTOUpdate.ClasesAlumno = new List<AlumnoClaseDTO>();
            var claseAux = this.Add(claseDTOUpdate.ActividadId, claseDTOUpdate);
            claseAux.ClasesAlumno = new List<AlumnoClaseDTO>();
            foreach (AlumnoClase alumnoClase in clase.ClasesAlumno)
            {
                this.alumnoService.agregarAlumnoAClase(alumnoClase.AlumnoId, claseAux.Id, alumnoClase.Tipo);
            }
            return claseAux;
        }
    }
}
