using BusinessLogic.Sessions.Validators;
using DataAccessInterface.Repositories;
using Domain;
using Dto;
using SessionInterface.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Sessions
{
    public class SessionLogic : ISessionLogic
    {
        private IRepository<User> userRepository;
        private SessionValidator sessionValidator;

        public SessionLogic(IRepository<User> userRepository, SessionValidator sessionValidator)
        {
            this.userRepository = userRepository;
            this.sessionValidator = sessionValidator;
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
