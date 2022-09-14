using UsersAuthExample.Response;

namespace UsersAuthExample.Services.ServiceResponses
{
    public class AuthenticateUserServiceResponse : ResponseBase
    {
        public string Token { get; set; }
    }
}
