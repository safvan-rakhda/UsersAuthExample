using System.Threading;
using System.Threading.Tasks;
using UsersAuthExample.Services.ServiceRequest;
using UsersAuthExample.Services.ServiceResponses;

namespace UsersAuthExample.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetUserById(int userId, CancellationToken cancellationToken = default);

        public Task<User> CreateUser(CreateUserServiceRequest request, CancellationToken cancellationToken = default);

        public Task<AuthenticateUserServiceResponse> AuthenticateUser(AuthenticateUserServiceRequest request, CancellationToken cancellationToken = default);
    }
}
