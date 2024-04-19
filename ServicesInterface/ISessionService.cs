using Dto;
using System;

namespace ServicesInterface
{
    public interface ISessionService
    {
        bool IsCorrectToken(Guid token);

        Guid Login(UserLoginDTO userLoginDto);

        UserDTO GetUser(string userName);
    }
}
