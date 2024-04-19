using BusinessLogic.Sessions;
using BusinessLogic.Sessions.Validators;
using BusinessLogic.Users.Validators;
using DataAccess.Context;
using DataAccess.Repositories;
using DataAccessInterface.Repositories;
using Domain;
using Mapper.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Services;
using ServicesInterface;
using SessionInterface.Sessions;

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
        }

        private void AddServices()
        {
            services.AddScoped<ISessionLogic, SessionLogic>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IUserService, UserService>();
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
