using AutoMapper;
using Commons.Exceptions;
using DataAccessInterface.Repositories;
using Domain;
using Dto;
using Dto.Profesores;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class ActividadService : IActividadService
    {
        private IProfeRepository profeRepository;
        private IActividadRepository actividadRepository;
        private IRepository<Local> localRepository;
        private IRepository<Agenda> agendaRepository;
        private IMapper mapper;

        public ActividadService(IMapper mapper, IProfeRepository profeRepository, IRepository<Local> localRepository, IActividadRepository actividadRepository, IRepository<Agenda> agendaRepository)
        {
            this.profeRepository = profeRepository;
            this.localRepository = localRepository;
            this.actividadRepository = actividadRepository;
            this.agendaRepository = agendaRepository;
            this.mapper = mapper;

        }

        public ActividadDTO Add(ActividadDTO actividadDTO)
        {
            Actividad actividad = new Actividad
            {
                Nombre = actividadDTO.Nombre,
                DescripcionCorta = actividadDTO.DescripcionCorta,
                Descripcion = actividadDTO.Descripcion,
                Activo = actividadDTO.Activo,
                Color = actividadDTO.Color,
            };
            var profesores = new List<Profesor>();
            var locales = new List<Local>();

            if (actividadDTO.Profesores != null)
            {
                foreach (ProfesorDTO profesor in actividadDTO.Profesores)
                {
                    if (profesor.Id != 0)
                    {
                        Profesor prfeRepo = this.profeRepository.getProfesores().FirstOrDefault(p => p.Id == profesor.Id);
                        profesores.Add(prfeRepo);
                    }
                }
            }
            actividad.Profesores = profesores;

            if (actividadDTO.Locales != null)
            {
                foreach (LocalDTO local in actividadDTO.Locales)
                {
                    if (local.Id != 0)
                    {
                        Local localRepo = this.localRepository.List().FirstOrDefault(a => a.Id == local.Id);
                        locales.Add(localRepo);
                    }
                }
            }
            actividad.Locales = locales;
            this.actividadRepository.AddAndSave(actividad);
            return this.mapper.Map<ActividadDTO>(actividad);
        }
        public IEnumerable<ActividadDTO> GetAll()
        {
            var actividades = this.actividadRepository.IncludeAll("Profesores");
            return this.mapper.Map<IEnumerable<ActividadDTO>>(actividades);
        }

        //solo los profesores de la actividad
        public ActividadDTO GetId(int actividadId)
        {
            var actividad = this.actividadRepository.IncludeAll("Profesores","Locales","Clases", "Planes").FirstOrDefault(a => a.Id == actividadId);
            return this.mapper.Map<ActividadDTO>(actividad);
        }
        public ActividadDTO GetLightId(int actividadId)
        {
            var actividad = this.actividadRepository.IncludeAll().FirstOrDefault(a => a.Id == actividadId);
            return this.mapper.Map<ActividadDTO>(actividad);
        }
        public IEnumerable<ProfesorLightDTO> GetProfesores(int actividadId)
        {
            var actividades = this.actividadRepository.IncludeAll("Profesores").FirstOrDefault(a => a.Id == actividadId);

            return this.mapper.Map<IEnumerable<ProfesorLightDTO>>(actividades.Profesores);
        }
        public IEnumerable<AgendaDTO> GetClases(int actividadId, DateTime desde, DateTime hasta)
        {
            var agendas = this.agendaRepository
                .IncludeAll("Clase.Profesor")
                .Where(a => a.Clase.Actividad.Id == actividadId
                         && a.Clase.HorarioInicio >= desde
                         && a.Clase.HorarioInicio <= hasta)
                .OrderBy(a => a.Clase.HorarioInicio)  // ✔ fecha + hora en una sola ordenación
                .ToList();

            return this.mapper.Map<IEnumerable<AgendaDTO>>(agendas);
        }
        public void Remove(int actividadId)
        {
            var actividad = this.actividadRepository.GetAll().FirstOrDefault(a => a.Id == actividadId);
            if (actividad == null)
            {
                throw new Exception("No existe la actividad seleccionado.");
            }
            actividad.Activo = false;
            this.actividadRepository.Update(actividad);
        }
        public ActividadDTO Activar(int actividadId, bool activar)
        {
            var actividad = this.actividadRepository.GetAll().FirstOrDefault(a => a.Id == actividadId);
            if (actividad == null)
            {
                throw new Exception("No existe la actividad seleccionado.");
            }
            actividad.Activo = activar;
            this.actividadRepository.Update(actividad);
            return this.mapper.Map<ActividadDTO>(actividad);
        }
        public ActividadDTO Update(ActividadDTO actividadDTOUpdate)
        {
            var profesores = new List<Profesor>();
            var locales = new List<Local>();
            Actividad actividadToUpdate = actividadRepository.IncludeAll("Profesores", "Locales").FirstOrDefault(a => a.Id == actividadDTOUpdate.Id);
            if (actividadToUpdate == null)
            {
                throw new ValidationException("No existe la actividad seleccionado.");
            }

            if (actividadDTOUpdate.Profesores != null)
            {
                foreach (ProfesorDTO profesor in actividadDTOUpdate.Profesores)
                {
                    if (profesor.Id != 0)
                    {
                        Profesor prfeRepo = this.profeRepository.getProfesores().FirstOrDefault(p => p.Id == profesor.Id);
                        profesores.Add(prfeRepo);
                    }
                }
            }
            actividadToUpdate.Profesores = profesores;

            if (actividadDTOUpdate.Locales != null)
            {
                foreach (LocalDTO local in actividadDTOUpdate.Locales)
                {
                    if (local.Id != 0)
                    {
                        Local localRepo = this.localRepository.List().FirstOrDefault(a => a.Id == local.Id);
                        locales.Add(localRepo);
                    }
                }
            }
            actividadToUpdate.Locales = locales;

            actividadToUpdate.Nombre = actividadDTOUpdate.Nombre;
            actividadToUpdate.DescripcionCorta = actividadDTOUpdate.DescripcionCorta;
            actividadToUpdate.Descripcion = actividadDTOUpdate.Descripcion;
            actividadToUpdate.Color = actividadDTOUpdate.Color;
            actividadToUpdate.Activo = actividadDTOUpdate.Activo;       
            this.actividadRepository.Update(actividadToUpdate);
            return this.mapper.Map<ActividadDTO>(actividadToUpdate);
        }

        public IEnumerable<ActividadDTO> GetPorLocal(int localId)
        {
            var actividades = this.actividadRepository.IncludeAll("Profesores").Where(a => a.Activo == true && a.Locales.Any(al => al.Id== localId));
            return this.mapper.Map<IEnumerable<ActividadDTO>>(actividades);
        }
    }
}
