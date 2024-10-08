﻿using AutoMapper;
using DataAccessInterface.Repositories;
using Domain;
using Domain.Alumnos;
using Dto.Clases;
using Dto.DashBoard;
using ServicesInterface;
using System;

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
