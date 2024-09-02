using AutoMapper;
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
    public class PlanService:IPlanService
    {
        private IRepository<Plan> planRepository;
        private IRepository<Actividad> actividadRepository;
        private IMapper mapper;
        public PlanService(IMapper mapper, IRepository<Plan> planRepository, IRepository<Actividad> actividadRepository)
        {
            this.planRepository = planRepository;
            this.actividadRepository = actividadRepository;
            this.mapper = mapper;
        }
        public PlanDTO Add(PlanDTO planDTO)
        {
            Plan plan = new Plan
            {
                Nombre = planDTO.Nombre,
                Tipo= (Plan.tipo)planDTO.Tipo,
                Descripcion = planDTO.Descripcion,
                Precio= planDTO.Precio,
                VecesxSemana=planDTO.VecesxSemana ?? 0,
                VecesxMes=planDTO.VecesxMes ?? 0,
                Activo =true,
                ActividadLibreId=planDTO.ActividadLibreId
            };
            var actividades = new List<Actividad>();

            if (planDTO.Actividades != null)
            {
                foreach (ActividadDTO actividad in planDTO.Actividades)
                {
                    if (actividad.Id != 0)
                    {
                        Actividad actRepo = this.actividadRepository.List().FirstOrDefault(a => a.Id == actividad.Id);
                        actividades.Add(actRepo);
                    }
                }
            }
            plan.Actividades = actividades;
            this.planRepository.AddAndSave(plan);
            return this.mapper.Map<PlanDTO>(plan);
        }

        public IEnumerable<PlanDTO> GetAll()
        {
            var planes = this.planRepository.List();
            return this.mapper.Map<IEnumerable<PlanDTO>>(planes);
        }

        public PlanDTO GetId(int planId)
        {
            var plan = this.planRepository.IncludeAll("Actividades").FirstOrDefault(p => p.Id == planId);
            return this.mapper.Map<PlanDTO>(plan);
        }

        public void Remove(int planId)
        {
            var plan = this.planRepository.List().FirstOrDefault(p => p.Id == planId);
            if (plan == null)
            {
                throw new Exception("No existe el local seleccionado.");
            }
            plan.Activo = false;
            this.planRepository.Update(plan); ;
        }
        public PlanDTO Activar(int planId, bool activar)
        {
            var plan = this.planRepository.List().FirstOrDefault(p => p.Id == planId);
            if (plan == null)
            {
                throw new Exception("No existe el plan seleccionado.");
            }
            plan.Activo = activar;
            this.planRepository.Update(plan);
            return this.mapper.Map<PlanDTO>(plan);
        }
        public PlanDTO Update(PlanDTO planDTO)
        {
            var planToUpdate = this.planRepository.IncludeAll("Actividades").FirstOrDefault(p => p.Id == planDTO.Id);
            planToUpdate.Nombre = planDTO.Nombre;
            planToUpdate.Tipo = (Plan.tipo)planDTO.Tipo;
            planToUpdate.Descripcion = planDTO.Descripcion;
            planToUpdate.Precio = planDTO.Precio;
            planToUpdate.VecesxSemana = planDTO.VecesxSemana ?? 0;
            planToUpdate.VecesxMes = planDTO.VecesxMes ?? 0;
            planToUpdate.ActividadLibreId = planDTO.ActividadLibreId;
            var actividades = new List<Actividad>();

            if (planDTO.Actividades != null)
            {
                foreach (ActividadDTO actividad in planDTO.Actividades)
                {
                    if (actividad.Id != 0)
                    {
                        Actividad actRepo = this.actividadRepository.List().FirstOrDefault(a => a.Id == actividad.Id);
                        actividades.Add(actRepo);
                    }
                }
            }
            planToUpdate.Actividades = actividades;
            this.planRepository.Update(planToUpdate);
            return this.mapper.Map<PlanDTO>(planToUpdate);
        }


    }
}
