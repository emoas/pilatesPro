using AutoMapper;
using BusinessLogic.Sessions.Validators;
using DataAccessInterface.Repositories;
using Domain;
using Dto;
using ServicesInterface;
using System;
using System.Linq;

namespace Services
{
    public class SessionService : ISessionService
    {
        private IRepository<User> userRepository;
        private SessionValidator sessionValidator;
        private IMapper mapper;

        public SessionService(IRepository<User> userRepository, SessionValidator sessionValidator, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.sessionValidator = sessionValidator;
            this.mapper = mapper;
        }

        public UserDTO GetUser(string userName)
        {
            var user = this.userRepository.List().FirstOrDefault(u => u.Email == userName);
            return this.mapper.Map<UserDTO>(user);
        }

        public bool IsCorrectToken(Guid token)
        {
            return this.userRepository.List().ToList().Exists(u => u.Token == token);
        }

        public Guid Login(UserLoginDTO userLoginDto)
        {
            this.sessionValidator.ValidateLogin(userLoginDto);
            var user = this.SearchUser(userLoginDto);
            this.sessionValidator.ValidateExistingUser(user);
            if (user!=null &&user.Rol==User.rol.ALUMNO)
                this.sessionValidator.ValidateActiveUser(user);         
            return user.Token;
        }
        private User SearchUser(UserLoginDTO userLoginDto)
        {
            var user = this.userRepository.List().FirstOrDefault(u =>
                u.Email.ToLower().Equals(userLoginDto.Email.ToLower())
                && u.Password.Equals(userLoginDto.Password));

            return user;
        }
    }
}
