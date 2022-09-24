using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UsersAuthExample.Data.Dto;
using UsersAuthExample.Data.Interfaces;
using UsersAuthExample.Services.ServiceRequest;

namespace UsersAuthExample.Data
{
    public class UserDataStore : IUserDataStore
    {
        private readonly IConfiguration _configuration;
        private readonly string _spGetUserById = "apisp_getUserById";
        private readonly string _spCreateUser = "apisp_createUser";
        private readonly string _spGetUserByUsername = "apisp_getUserByUsername";
        private readonly string _spGetUsers = "apisp_getUsers";
        private readonly string _spGetDeleteUsers = "apisp_DeleteUsers";

        public UserDataStore(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<UserDto> CreateUser(CreateUserServiceRequest request, CancellationToken cancellationToken = default)
        {
            var spParams = new
            {
                username = request.UserName,
                firstname = request.FirstName,
                lastname = request.LastName,
                role = request.Role,
                password = request.Password,
                secret = request.Secrect
            };
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("Sql.Users"));
                var userId = await connection.QueryFirstOrDefaultAsync<int>(_spCreateUser, spParams
                    , commandType: CommandType.StoredProcedure);

                return await GetUserById(userId, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDto> GetUserById(int userId, CancellationToken cancellationToken = default)
        {
            IEnumerable<UserDto> users;
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("Sql.Users"));
                users = await connection.QueryAsync<UserDto>(_spGetUserById, new { user_id = userId }
                , commandType: CommandType.StoredProcedure);

                return users.ToList().FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserToAuthenticateDto> GetUserToAuthenticate(string username, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("Sql.Users"));
                var users = await connection.QueryAsync<UserToAuthenticateDto>(_spGetUserByUsername, new { username }, commandType: System.Data.CommandType.StoredProcedure);
                return users.ToList().FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserDto>> GetUsers(CancellationToken cancellationToken = default)
        {
            IEnumerable<UserDto> users;
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("Sql.Users"));
                users = await connection.QueryAsync<UserDto>(_spGetUsers, commandType: CommandType.StoredProcedure);

                return users.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async void DeleteUsersAsync(int[] userIds, CancellationToken cancellationToken = default)
        {
            try
            {
                //TODO: Call _spGetDeleteUsers SP
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
