using AutoMapper;
using DataAccessInterface.Repositories;
using Domain;
using Dto.Reports;
using ServicesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class ReportService:IReportService
    {
        private IRepository<Plan> planRepository;
        private IRepository<Actividad> actividadRepository;
        private IClaseRepository claseRepository;
        private IMapper mapper;
        public ReportService(IMapper mapper, IRepository<Plan> planRepository, IRepository<Actividad> actividadRepository,IClaseRepository claseRepository)
        {
            this.planRepository = planRepository;
            this.actividadRepository = actividadRepository;
            this.claseRepository = claseRepository;
            this.mapper = mapper;
        }

        public List<ReservaPorPlanDTO> GetReservasPorPLan(int planId, DateTime desde, DateTime hasta)
        {
            var result = new List<ReservaPorPlanDTO>();

            // Obtener todas las clases que cumplan con los criterios
            var reservas = this.claseRepository.IncludeAllAnidado("Local", "Actividad", "ClasesAlumno.Alumno")
                .Where(c => c.Activo == true &&
                            c.HorarioInicio.Date >= desde &&
                            c.HorarioFin.Date <= hasta &&
                            c.ClasesAlumno.Any(ca => ca.Alumno.PlanId == planId))
                .ToList();
            // Iterar sobre las clases y extraer las reservas de los alumnos con el plan especificado
            foreach (var clase in reservas)
            {
                foreach (var claseAlumno in clase.ClasesAlumno.Where(ca => ca.Alumno.PlanId == planId))
                {
                    var reserva = new ReservaPorPlanDTO
                    {
                        AlumnoId = claseAlumno.Alumno.Id,
                        AlumnoNombre = claseAlumno.Alumno.Name,
                        AlumnoApellido = claseAlumno.Alumno.Apellido,
                        ClaseId = clase.Id,
                        ActividadNombre = clase.Actividad.Nombre,
                        LocalNombre = clase.Local.Nombre,
                        HorarioInicio = clase.HorarioInicio,
                        HorarioFin = clase.HorarioFin
                    };

                    result.Add(reserva);
                }
            }
            return result;
        }
    }
}
