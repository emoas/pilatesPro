using AutoMapper;
using Domain;
using Domain.Alumnos;
using Domain.DashBoard;
using Dto;
using Dto.Alumnos;
using Dto.DashBoard;
using Dto.Profesores;

namespace Mapper.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<LocalDTO, Local>();
            CreateMap<Local, LocalDTO>();
            CreateMap<ProfesorDTO, Profesor>();
            CreateMap<Profesor, ProfesorDTO>();
            CreateMap<ProfesorLightDTO, Profesor>();
            CreateMap<Profesor, ProfesorLightDTO>();
            CreateMap<ProfesorDTO, User>();
            CreateMap<User, ProfesorDTO>();
            CreateMap<ProfesorLightDTO, User>();
            CreateMap<User, ProfesorLightDTO>();
            CreateMap<AlumnoDTO, Alumno>();
            CreateMap<Alumno, AlumnoDTO>();
            CreateMap<AlumnoDTO, User>();
            CreateMap<User, AlumnoDTO>();
            CreateMap<ActividadDTO, Actividad>();
            CreateMap<Actividad, ActividadDTO>();
            CreateMap<ClaseDTO, Clase>();
            CreateMap<Clase, ClaseDTO>();
            CreateMap<Agenda, AgendaDTO>();
            CreateMap<AgendaDTO, Agenda>();
            CreateMap<Patologia, PatologiaDTO>();
            CreateMap<PatologiaDTO, Patologia>();
            CreateMap<Plan, PlanDTO>();
            CreateMap<PlanDTO, Plan>();
            CreateMap<ClaseFija, ClaseFijaDTO>();
            CreateMap<ClaseFijaDTO, ClaseFija>();
            CreateMap<AlumnoClase, AlumnoClaseDTO>();
            CreateMap<AlumnoClaseDTO, AlumnoClase>();
            CreateMap<DashBoard, DashBoardDTO>();
            CreateMap<DashBoardDTO, DashBoard>();
            CreateMap<CupoPendiente, CupoPendienteDTO>();
            CreateMap<CupoPendienteDTO, CupoPendiente>();
            CreateMap<LicenciaAlumno, LicenciaAlumnoDTO>();
            CreateMap<LicenciaAlumnoDTO, LicenciaAlumno>();
        }
    }
}
