using Dto;
using System;

namespace ServicesInterface
{
    public interface IUserService
    {
        UserDTO Get(Guid token);
        UserDTO Add(UserDTO userCreateDto);
        UserDTO Update(UserDTO userUpdateDto);
        void Remove(int userId);
    }
}
