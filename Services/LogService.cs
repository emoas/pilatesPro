using AutoMapper;
using DataAccessInterface.Repositories;
using Domain.Logs;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class LogService : ILogService
    {
        private IRepository<Logs_AddAlumnoClase> logsAddAlumnoClaseRepository;
        private IMapper mapper;
        public LogService(IMapper mapper, IRepository<Logs_AddAlumnoClase> logsAddAlumnoClaseRepository)
        {
            this.logsAddAlumnoClaseRepository = logsAddAlumnoClaseRepository;
            this.mapper = mapper;

        }
        public Logs_AddAlumnoClase AddAlumnoClase(Logs_AddAlumnoClase logsAlumnoClase)
        {
            this.logsAddAlumnoClaseRepository.AddAndSave(logsAlumnoClase);
            return logsAlumnoClase;
        }
    }
}
