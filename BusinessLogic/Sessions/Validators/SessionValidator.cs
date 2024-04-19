using Commons.Exceptions;
using Domain;
using Dto;

namespace BusinessLogic.Sessions.Validators
{
    public class SessionValidator
    {
        public SessionValidator()
        {
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
    }
}
