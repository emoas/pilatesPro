using Commons.Exceptions;
using DataAccessInterface.Repositories;
using Domain;
using Dto;
using System.Linq;

namespace BusinessLogic.Sessions.Validators
{
    public class SessionValidator
    {
        private IRepository<User> userRepository;
        public SessionValidator(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public void ValidateLogin(UserLoginDTO userLoginDto)
        {
            if (userLoginDto.Email == null || userLoginDto.Password == null)
            {
                throw new ValidationException("Error, los campos enviados no pueden ser nulos.");
            }
        }

        public void ValidateExistingUser(User userToLogin)
        {
            if (userToLogin == null)
            {
                throw new ValidationException("Credenciales incorrectas.");
            }
        }
        public void ValidateActiveUser(User userToLogin)
        {
            if (!IsAlumnoActivo(userToLogin.Id))
            {
                throw new ValidationException("Su cuenta se encuentra inhabilitada...");
            }
        }
        private bool IsAlumnoActivo(int idAlumno)
        {
            return this.userRepository.List()
                .OfType<Alumno>()
                .Any(a => a.Id == idAlumno && a.Activo);  // Devuelve true si el alumno existe y está activo
        }
    }
}
