using AutoMapper;
using DataAccessInterface.Repositories;
using Domain;
using Domain.Alumnos;
using Dto;
using Dto.Clases;
using Dto.DashBoard;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class DashBoardService : IDashBoardService
    {
        private IActividadRepository actividadRepository;
        private IAlumnoRepository alumnoRepository;
        private IClaseService claseService;
        private IRepository<AlumnoClase> alumnoClaseRepository;
        private IMapper mapper;
        private IAgendaService agendaService;
        private IAlumnoService alumnoService;

        public DashBoardService(IMapper mapper, IAlumnoService alumnoService, IRepository<AlumnoClase> alumnoClaseRepository, IActividadRepository actividadRepository, IClaseService claseService, IAgendaService agendaService, IAlumnoRepository alumnoRepository)
        {
            this.actividadRepository = actividadRepository;
            this.claseService = claseService;
            this.agendaService = agendaService;
            this.alumnoService = alumnoService;
            this.alumnoClaseRepository = alumnoClaseRepository;
            this.alumnoRepository = alumnoRepository;
            this.mapper = mapper;
        }

        public int GetAlumnosActivosPlan(int planId)
        {
            return this.alumnoRepository.IncludeAll("Plan")
               .Where(a => a.Activo && a.Plan.Id == planId)
               .Count();
        }
        public int GetAlumnosActivosDirectos()
        {
            return this.alumnoRepository.IncludeAll("Plan")
               .Where(a => a.Activo && a.Plan.Id != 39 && a.Plan.Id != 40)
               .Count();
        }
        public IEnumerable<AgendaDTO> GetClasesLocalFecha(int idLocal, DateTime fecha)
        {
            var agendas = this.agendaService.GetPorFecha(idLocal,fecha)
                                            .OrderBy(a => a.HorarioInicio); // Ordena por la propiedad de fecha;
            return this.mapper.Map<IEnumerable<AgendaDTO>>(agendas);
        }

        public IEnumerable<ClaseDTO> GetClasesProfeFecha(int idProfe, DateTime fecha)
        {
            var clases = this.claseService.GetClasesPorFecha(fecha).Where(c => c.Profesor.Id == idProfe)
                                            .OrderBy(c => c.HorarioInicio); // Ordena por la propiedad de fecha;
            return this.mapper.Map<IEnumerable<ClaseDTO>>(clases);
        }

        public DashBoardDTO GetHome()
        {
            DashBoardDTO dash = new DashBoardDTO();
            ClasesTodayDTO clasesToday= new ClasesTodayDTO();
            clasesToday = this.claseService.GetClasesToday(DateTime.Now.Date);
            dash.TotalClasesToday = clasesToday.CantidadClasesHoy;
            dash.TotalAgendadosHoy = clasesToday.SumaCuposOtorgadosHoy;
            return dash;
        }
        public IEnumerable<AlumnoDTO> GetPaseLibre2Faltas()
        {
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var alumnos = this.alumnoRepository.IncludeAll()
                .Where(a => a.Plan.Id == 39 &&
                            a.Faltas.Count(f => f.Fecha >= startOfMonth && f.Fecha <= endOfMonth) >= 2)
                .ToList();

            return this.mapper.Map<IEnumerable<AlumnoDTO>>(alumnos);
        }

        public int GetReservasWeb(int mes)
        {
            int year = DateTime.Now.Year;
            return this.alumnoClaseRepository.List()
                .Where(ac => ac.Fecha.HasValue && ac.Fecha.Value.Month == mes && ac.Fecha.Value.Year == year && ac.Tipo == AlumnoClase.tipo.WEB)
                .Count();
        }
    }
}
