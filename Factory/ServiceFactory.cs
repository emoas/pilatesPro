using BusinessLogic.Sessions;
using BusinessLogic.Sessions.Validators;
using BusinessLogic.Users.Validators;
using DataAccess.Context;
using DataAccess.Repositories;
using DataAccessInterface.Repositories;
using Domain;
using Domain.Alumnos;
using Domain.Logs;
using Mapper.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Services;
using ServicesInterface;
using SessionInterface.Sessions;
using System.Net.Http;

namespace Factory
{
    public class ServiceFactory
    {
        private readonly IServiceCollection services;

        public ServiceFactory(IServiceCollection services)
        {
            this.services = services;
        }

        public void AddCustomServices()
        {
            this.AddRepositories();
            this.AddServices();
            services.AddScoped<UserValidator, UserValidator>();
            services.AddScoped<SessionValidator, SessionValidator>();
        }

        private void AddRepositories()
        {
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<Local>, Repository<Local>>();
            services.AddScoped<IRepository<Actividad>, Repository<Actividad>>();
            services.AddScoped<IRepository<Patologia>, Repository<Patologia>>();
            services.AddScoped<IRepository<Agenda>, Repository<Agenda>>();
            services.AddScoped<IRepository<Clase>, Repository<Clase>>();
            services.AddScoped<IRepository<AlumnoClase>, Repository<AlumnoClase>>();
            services.AddScoped<IRepository<ClaseFija>, Repository<ClaseFija>>();
            services.AddScoped<IProfeRepository, ProfeRepository>();
            services.AddScoped<IAlumnoRepository, AlumnoRepository>();
            services.AddScoped<IRepository<Plan>, Repository<Plan>>();
            services.AddScoped<IActividadRepository, ActividadRepository>();
            services.AddScoped<IAlumnoClaseRepository, AlumnoClaseRepository>();
            services.AddScoped<IClaseRepository, ClaseRepository>();
            services.AddScoped<IRepository<ClaseFija>, Repository<ClaseFija>>();
            services.AddScoped<IRepository<Falta>, Repository<Falta>>();
            services.AddScoped<IRepository<LicenciaAlumno>, Repository<LicenciaAlumno>>();
            services.AddScoped<IRepository<CupoPendiente>, Repository<CupoPendiente>>();
            services.AddScoped<IRepository<Logs_AddAlumnoClase>, Repository<Logs_AddAlumnoClase>>();
        }

        private void AddServices()
        {
            services.AddScoped<ISessionLogic, SessionLogic>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILocalService, LocalService>();
            services.AddScoped<IProfesorService, ProfesorService>();
            services.AddScoped<IAlumnoService, AlumnoService>();
            services.AddScoped<IActividadService, ActividadService>();
            services.AddScoped<IClaseService, ClaseService>();
            services.AddScoped<IAgendaService, AgendaService>();
            services.AddScoped<IPatologiaService, PatologiaService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IDashBoardService, DashBoardService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<HttpClient>();

        }

        public void AddDbContextService()
        {
            services.AddDbContext<DbContext, PilatesContext>();
        }

        public void AddAutoMapperServices()
        {
            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
