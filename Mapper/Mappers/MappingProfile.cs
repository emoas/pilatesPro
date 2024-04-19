using AutoMapper;
using Domain;
using Dto;

namespace Mapper.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

        }
    }
}
