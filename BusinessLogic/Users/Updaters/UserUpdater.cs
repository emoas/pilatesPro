using DataAccessInterface.Repositories;
using Domain;
using Dto;

namespace BusinessLogic.Users.Updaters
{
    public class UserUpdater
    {
        private readonly IRepository<User> userRepository;

        public UserUpdater(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public void Update(UserDTO userUpdate, User userToUpdate)
        {
            userToUpdate.Name = userUpdate.Name;
            userToUpdate.Email = userUpdate.Email;
            userToUpdate.Password = userUpdate.Password;

            this.userRepository.Update(userToUpdate);
        }
    }
}
