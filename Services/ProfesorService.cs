using AutoMapper;
using Commons.Exceptions;
using DataAccessInterface.Repositories;
using Domain;
using Dto;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class ProfesorService : IProfesorService
    {
        private IProfeRepository profeRepository;
        private IRepository<Patologia> patologiaRepository;
        private IRepository<Actividad> actividadRepository;
        private IRepository<Local> localRepository;
        private IRepository<Agenda> agendaRepository;
        private IMapper mapper;
        public ProfesorService(IMapper mapper, IProfeRepository profeRepository, IRepository<Patologia> patologiaRepository, IRepository<Local> localRepository, IRepository<Actividad> actividadRepository, IRepository<Agenda> agendaRepository)
        {
            this.profeRepository = profeRepository;
            this.patologiaRepository = patologiaRepository;
            this.localRepository = localRepository;
            this.actividadRepository = actividadRepository;
            this.agendaRepository = agendaRepository;
            this.mapper = mapper;

        }
        public ProfesorDTO Add(ProfesorDTO profeDTO)
        {
            Profesor profe = new Profesor
            {
                Name = profeDTO.Name,
                Apellido = profeDTO.Apellido,
                Sobrenombre=profeDTO.Sobrenombre,
                Email = profeDTO.Email,
                Cedula = profeDTO.Cedula,
                Direccion = profeDTO.Direccion,
                Celular = profeDTO.Celular,
                Ciudad = profeDTO.Ciudad,
                EmeregenciaMovil = profeDTO.EmeregenciaMovil,
                ContactoEmergencia = profeDTO.ContactoEmergencia,
                TelefonoContacto = profeDTO.TelefonoContacto,
                Activo = profeDTO.Activo,
                FechaNacimiento = profeDTO.FechaNacimiento
            };
            var patologias = new List<Patologia>();
            var actividades = new List<Actividad>();
            var locales = new List<Local>();

            if (profeDTO.PatologíasQuePresenta != null)
            {
                foreach (PatologiaDTO patologia in profeDTO.PatologíasQuePresenta)
                {
                    if (patologia.Id != 0)
                    {
                        Patologia patologiaRepo = this.patologiaRepository.List().FirstOrDefault(p => p.Id == patologia.Id);
                        patologias.Add(patologiaRepo);
                    }
                }
            }
            profe.PatologíasQuePresenta = patologias;

            if (profeDTO.Actividades != null)
            {
                foreach (ActividadDTO actividad in profeDTO.Actividades)
                {
                    if (actividad.Id != 0)
                    {
                        Actividad actividadRepo = this.actividadRepository.List().FirstOrDefault(a => a.Id == actividad.Id);
                        actividades.Add(actividadRepo);
                    }
                }
            }
            profe.Actividades = actividades;

            if (profeDTO.Locales != null)
            {
                foreach (LocalDTO local in profeDTO.Locales)
                {
                    if (local.Id != 0)
                    {
                        Local localRepo = this.localRepository.List().FirstOrDefault(a => a.Id == local.Id);
                        locales.Add(localRepo);
                    }
                }
            }
            profe.Locales = locales;
            this.profeRepository.AddAndSave(profe);
            return this.mapper.Map<ProfesorDTO>(profe);
        }

        public IEnumerable<ProfesorDTO> GetAll()
        {
            var profesores=profeRepository.getProfesores().Where(p => p.Rol==User.rol.PROFE)
                                .OrderByDescending(p => p.Activo)
                                .ToList();
            return this.mapper.Map<IEnumerable<ProfesorDTO>>(profesores);
        }

        public IEnumerable<AgendaDTO> GetClases(int profeId, DateTime desde, DateTime hasta)
        {
            var agendas = this.agendaRepository.IncludeAll("Clase.Profesor").Where(
               a => a.Clase.Profesor.Id == profeId
               && a.Clase.HorarioInicio.Date >= desde.Date
               && a.Clase.HorarioInicio.Date <= hasta.Date)
               .ToList();
            return this.mapper.Map<IEnumerable<AgendaDTO>>(agendas);
        }

        public ProfesorDTO GetId(int profeId)
        {
            var profe = this.profeRepository.IncludeAll("Actividades", "Locales").FirstOrDefault(l => l.Id == profeId);
            return this.mapper.Map<ProfesorDTO>(profe);
        }

        public void Remove(int personalId)
        {
            Profesor profe = (Profesor)this.profeRepository.getProfesores().FirstOrDefault(p => p.Id == personalId);
            if (profe == null)
            {
                throw new Exception("No existe el profesor seleccionado.");
            }
            if (profe.Activo)
            {
                //Al desactivar el alumno lo elimino las clases fijas asigandas
                profe.Activo = false;
            }
            else
            {
                profe.Activo = true;
            }
            this.profeRepository.Update(profe);    
        }

        public ProfesorDTO Update(ProfesorDTO profesorDTOUpdate)
        {
            var patologias = new List<Patologia>();
            var actividades = new List<Actividad>();
            var locales = new List<Local>();

            Profesor profeToUpdate = profeRepository.IncludeAll("Actividades", "Locales", "PatologíasQuePresenta").FirstOrDefault(p => p.Id == profesorDTOUpdate.Id);
            if (profeToUpdate == null)
            {
                throw new ValidationException("No existe el profesor seleccionado.");
            }

            //agrego las patologias
            if (profesorDTOUpdate.PatologíasQuePresenta != null)
            {
                foreach (PatologiaDTO patologia in profesorDTOUpdate.PatologíasQuePresenta)
                {
                    if (patologia.Id != 0)
                    {
                        Patologia patologiaRepo = this.patologiaRepository.List().FirstOrDefault(p => p.Id == patologia.Id);
                        patologias.Add(patologiaRepo);
                    }
                }
            }
            profeToUpdate.PatologíasQuePresenta = patologias;

            //agrego las actividades
            if (profesorDTOUpdate.Actividades != null)
            {
                foreach (ActividadDTO actividad in profesorDTOUpdate.Actividades)
                {
                    if (actividad.Id != 0)
                    {
                        Actividad actividadRepo = this.actividadRepository.List().FirstOrDefault(a => a.Id == actividad.Id);
                        actividades.Add(actividadRepo);
                    }
                }
            }
            profeToUpdate.Actividades = actividades;

            //agrego los locales
            if (profesorDTOUpdate.Locales != null)
            {
                foreach (LocalDTO local in profesorDTOUpdate.Locales)
                {
                    if (local.Id != 0)
                    {
                        Local localRepo = this.localRepository.List().FirstOrDefault(a => a.Id == local.Id);
                        locales.Add(localRepo);
                    }
                }
            }
            profeToUpdate.Locales = locales;

            profeToUpdate.Name = profesorDTOUpdate.Name;
            profeToUpdate.Apellido = profesorDTOUpdate.Apellido;
            profeToUpdate.Sobrenombre = profesorDTOUpdate.Sobrenombre;
            profeToUpdate.Cedula = profesorDTOUpdate.Cedula;
            profeToUpdate.Direccion = profesorDTOUpdate.Direccion;
            profeToUpdate.Celular = profesorDTOUpdate.Celular;
            profeToUpdate.Ciudad = profesorDTOUpdate.Ciudad;
            profeToUpdate.EmeregenciaMovil = profesorDTOUpdate.EmeregenciaMovil;
            profeToUpdate.ContactoEmergencia = profesorDTOUpdate.ContactoEmergencia;
            profeToUpdate.TelefonoContacto = profesorDTOUpdate.TelefonoContacto;
            profeToUpdate.Activo = profesorDTOUpdate.Activo;
            profeToUpdate.FechaNacimiento = profesorDTOUpdate.FechaNacimiento;
            this.profeRepository.Update(profeToUpdate);
            return this.mapper.Map<ProfesorDTO>(profeToUpdate);
        }
    }
}
