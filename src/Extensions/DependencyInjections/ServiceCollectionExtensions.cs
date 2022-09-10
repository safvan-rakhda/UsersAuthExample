using Microsoft.Extensions.DependencyInjection;
using System;
using UsersAuthExample.Data;
using UsersAuthExample.Data.Interfaces;
using UsersAuthExample.Services;
using UsersAuthExample.Services.Interfaces;

namespace UsersAuthExample.Extensions.DependencyInjections
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterDependency(this IServiceCollection services)
        {
            services.AddScoped<IUserDataStore, UserDataStore>()
            .AddScoped<IUserService, UserService>()
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
