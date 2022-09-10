using AutoMapper;
using UsersAuthExample.Data.Dto;
using UsersAuthExample.Request;
using UsersAuthExample.Services.ServiceRequest;
using UsersAuthExample.Services.ServiceResponses;

namespace UsersAuthExample.Mappings.AutoMapper
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<CreateUserApiRequest, CreateUserServiceRequest>();
        }
    }
}
