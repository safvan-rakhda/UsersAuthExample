using AutoMapper;
using System;
using UsersAuthExample.Auth;
using UsersAuthExample.Data.Dto;
using UsersAuthExample.Request;
using UsersAuthExample.Response;
using UsersAuthExample.Services.ServiceRequest;
using UsersAuthExample.Services.ServiceResponses;

namespace UsersAuthExample.Mappings.AutoMapper
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<CreateUserApiRequest, CreateUserServiceRequest>()
                .ForMember(s => s.Secrect, opt => opt.MapFrom(src => Encryption.CreateSecret()));
            //TODO: Compute password hash in mapper
            //.ForMember(s => s.Password, opt => opt.MapFrom(
            //    (source, dest) =>
            //    {
            //        return Encryption.ComputePasswordHash(source.Password, dest.Secrect);
            //    })
            //)

            CreateMap<UserSignInApiRequest, AuthenticateUserServiceRequest>();
            CreateMap<AuthenticateUserServiceResponse, UserSignInApiResponse>();
            CreateMap<GetUsersServiceResponse, GetUsersApiResponse>();
        }
    }
}
