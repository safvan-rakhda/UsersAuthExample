using AutoMapper;
using System;
using System.Threading;
using System.Threading.Tasks;
using UsersAuthExample.Data.Interfaces;
using UsersAuthExample.Services.Interfaces;
using UsersAuthExample.Services.ServiceRequest;
using UsersAuthExample.Services.ServiceResponses;

namespace UsersAuthExample.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDataStore _userDataStore;
        private readonly IMapper _mapper;

        public UserService(IUserDataStore userDataStore, IMapper mapper)
        {
            _userDataStore = userDataStore;
            _mapper = mapper;
        }

        public async Task<User> CreateUser(CreateUserServiceRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userDataStore.CreateUser(request, cancellationToken);
                return _mapper.Map<User>(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetUserById(int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var user = await _userDataStore.GetUserById(userId, cancellationToken);
                return _mapper.Map<User>(user);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
