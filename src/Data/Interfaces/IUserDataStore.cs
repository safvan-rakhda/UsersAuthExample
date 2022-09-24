using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UsersAuthExample.Data.Dto;
using UsersAuthExample.Services.ServiceRequest;

namespace UsersAuthExample.Data.Interfaces
{
    public interface IUserDataStore
    {
        public Task<UserDto> GetUserById(int userId, CancellationToken cancellationToken = default);

        public Task<UserDto> CreateUser(CreateUserServiceRequest request
            , CancellationToken cancellationToken = default);

        public Task<UserToAuthenticateDto> GetUserToAuthenticate(string username
            , CancellationToken cancellationToken = default);

        public Task<List<UserDto>> GetUsers(CancellationToken cancellationToken = default);

        public void DeleteUsersAsync(int[] userIds, CancellationToken cancellationToken = default);
    }
}
