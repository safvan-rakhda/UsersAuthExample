using System.Collections.Generic;
using UsersAuthExample.Data.Dto;
using UsersAuthExample.Response;

namespace UsersAuthExample.Services.ServiceResponses
{
    public class GetUsersServiceResponse : ResponseBase
    {
        public IEnumerable<UserDto> Items { get; set; }
    }
}
