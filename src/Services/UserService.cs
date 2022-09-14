using AutoMapper;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UsersAuthExample.Auth;
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

        public async Task<AuthenticateUserServiceResponse> AuthenticateUser(AuthenticateUserServiceRequest request, CancellationToken cancellationToken = default)
        {
            AuthenticateUserServiceResponse serviceResponse = new();

            var dbUser = await _userDataStore.GetUserToAuthenticate(request.UserName, cancellationToken);

            if (dbUser == null || dbUser.UserId <= 0)
            {
                serviceResponse.Errors?.Add("AuthenticateUser", new[] { "Username is Incorrect." });
                serviceResponse.HttpStatusCode = HttpStatusCode.NotFound;
                return serviceResponse;
            }

            if (dbUser.Password != Encryption.ComputePasswordHash(request.Password, dbUser.Secret))
            {
                serviceResponse.Errors?.Add("AuthenticateUser", new[] { "Password is Incorrect." });
                serviceResponse.HttpStatusCode = HttpStatusCode.Unauthorized;
                return serviceResponse;
            }

            //TODO: compute accesstoken
            serviceResponse.Token = dbUser.Password;
            return serviceResponse;
        }

        public async Task<User> CreateUser(CreateUserServiceRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                request.Password = Encryption.ComputePasswordHash(request.Password, request.Secrect);
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
