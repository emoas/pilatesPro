using AutoMapper;
using BusinessLogic.Users.Updaters;
using BusinessLogic.Users.Validators;
using DataAccessInterface.Repositories;
using Domain;
using Dto;
using ServicesInterface;
using System;
using System.Linq;

namespace Services
{
    public class UserService : IUserService
    {
        private IRepository<User> userRepository;
        private IMapper mapper;
        private UserValidator userValidator;

        public UserService(IRepository<User> userRepository, IMapper mapper, UserValidator userValidator)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.userValidator = userValidator;
        }

        public UserDTO Get(Guid token)
        {
            var user = this.userRepository.List().FirstOrDefault(u => u.Token == token);
            return this.mapper.Map<UserDTO>(user);
        }

        public UserDTO Add(UserDTO userCreateDto)
        {
            this.userValidator.ValidateCreate(userCreateDto);

            var user = new User(userCreateDto.Name, userCreateDto.Email, userCreateDto.Password, (User.rol)userCreateDto.Rol);
            this.userRepository.AddAndSave(user);

            return this.mapper.Map<UserDTO>(user);
        }

        public UserDTO Update(UserDTO userUpdateDto)
        {
            var userToUpdate = this.SearchUser(userUpdateDto.Id);
            userValidator.ValidateUpdate(userUpdateDto, userToUpdate);

            var userUpdater = new UserUpdater(this.userRepository);
            userUpdater.Update(userUpdateDto, userToUpdate);

            return this.mapper.Map<UserDTO>(userToUpdate);
        }

        public void Remove(int userId)
        {
            var user = this.SearchUser(userId);
            this.userValidator.ValidateRemove(user);

            this.userRepository.Delete(user);
        }

        private User SearchUser(int userId)
        {
            return this.userRepository.List().FirstOrDefault(u => u.Id == userId);
        }
    }
}
