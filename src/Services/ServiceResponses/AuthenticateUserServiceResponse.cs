using UsersAuthExample.Response;

namespace UsersAuthExample.Services.ServiceResponses
{
    public class AuthenticateUserServiceResponse : ResponseBase
    {
        public string UserName { get; set; }

        public string Token { get; set; }
    }
}
