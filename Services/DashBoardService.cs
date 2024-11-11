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
        private IClaseService claseService;
        private IRepository<AlumnoClase> alumnoClaseRepository;
        private IMapper mapper;
        private IAgendaService agendaService;
        private IAlumnoService alumnoService;

        public DashBoardService(IMapper mapper, IAlumnoService alumnoService, IRepository<AlumnoClase> alumnoClaseRepository, IActividadRepository actividadRepository, IClaseService claseService, IAgendaService agendaService)
        {
            this.actividadRepository = actividadRepository;
            this.claseService = claseService;
            this.agendaService = agendaService;
            this.alumnoService = alumnoService;
            this.alumnoClaseRepository = alumnoClaseRepository;
            this.mapper = mapper;
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
    }
}
