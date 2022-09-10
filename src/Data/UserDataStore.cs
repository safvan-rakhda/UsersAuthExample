using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using UsersAuthExample.Data.Dto;
using UsersAuthExample.Data.Interfaces;
using System.Data.SqlClient;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using System;
using UsersAuthExample.Services.ServiceRequest;

namespace UsersAuthExample.Data
{
    public class UserDataStore : IUserDataStore
    {
        private readonly IConfiguration _configuration;
        private readonly string _spGetUserById = "apisp_getUserById";
        private readonly string _spCreateUser = "apisp_createUser";

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
                password = request.Password
            };
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("Sql.Users"));
                var userId = await connection.QueryFirstOrDefaultAsync<int>(_spCreateUser, spParams, commandType: System.Data.CommandType.StoredProcedure);

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
                users = await connection.QueryAsync<UserDto>(_spGetUserById, new { user_id = userId }, commandType: System.Data.CommandType.StoredProcedure);

                return users.ToList().FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
