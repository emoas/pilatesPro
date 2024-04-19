using Dto;
using System;

namespace SessionInterface.Sessions
{
    public interface ISessionLogic
    {
        bool IsCorrectToken(Guid token);

        Guid Login(UserLoginDTO userLoginDto);
    }
}
