using Dto;
using System;
using DataAccessInterface.Repositories;
using Commons.Exceptions;
using System.Linq;
using Domain;

namespace BusinessLogic.Users.Validators
{
    public class UserValidator
    {
       private IRepository<User> userRepository;

        public UserValidator(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public void ValidateCreate(UserDTO userToCreate)
        {
            if (userToCreate.Email == null)
            {
                throw new ValidationException("El mail del usuario que desea crear no puede ser nulo.");
            }

            this.ValidateExistsEmail(userToCreate.Email);
        }

        public void ValidateUpdate(UserDTO userWithUpdates, User userToUpdate)
        {
            this.ValidateIsNullEntity(userToUpdate);
            if (!userToUpdate.Email.Equals(userWithUpdates.Email))
            {
                this.ValidateExistsEmail(userWithUpdates.Email);
            }
        }

        public void ValidateRemove(User userToRemove)
        {
            this.ValidateIsNullEntity(userToRemove);
        }

        private void ValidateIsNullEntity(User user)
        {
            if (user == null)
            {
                throw new ValidationException("El usuario seleccionado no existe.");
            }
        }

        private void ValidateExistsEmail(string email)
        {
            if (this.userRepository.List().Any(u => u.Email.ToLower().Equals(email.ToLower())))
            {
                throw new ValidationException("El mail del usuario que desea crear ya existe.");
            }
        }
    }
}
