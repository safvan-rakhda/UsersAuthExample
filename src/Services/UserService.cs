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

            var user = await _userDataStore.GetUserById(dbUser.UserId, cancellationToken);
            serviceResponse.Token = await JwtHandler.GanerateToken(new GanerateTokenRequest() { UserId = user.UserId, Roles = new[] { user.Role } });
            serviceResponse.UserName = user.UserName;
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

        public async Task<GetUsersServiceResponse> GetUsers(CancellationToken cancellationToken = default)
        {
            try
            {
                var users = await _userDataStore.GetUsers(cancellationToken);
                return new GetUsersServiceResponse() { Items = users };
            }
            catch (Exception ex)
            {
                GetUsersServiceResponse response = new() { HttpStatusCode = HttpStatusCode.InternalServerError };
                response.Errors.Add("Error in SQL for GetUsers", new[] { ex.Message });
                return response;
            }
        }

        public async Task<string> DeleteUsersAsync(int[] userIds, CancellationToken cancellationToken = default)
        {
            try
            {
                _userDataStore.DeleteUsersAsync(userIds, cancellationToken);
                return "All user(s) deleted";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
