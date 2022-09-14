using Dapper.FluentMap.Mapping;
using UsersAuthExample.Data.Dto;

namespace UsersAuthExample.Mappings.Dapper
{
    public class UserToAuthenticateDtoMap: EntityMap<UserToAuthenticateDto>
    {
        public UserToAuthenticateDtoMap()
        {
            Map(u => u.UserId).ToColumn("c_UserId", false);
            Map(u => u.UserName).ToColumn("c_UserName", false);
            Map(u => u.Password).ToColumn("c_Password", false);
            Map(u => u.Secret).ToColumn("c_Secret", false);
        }
    }
}
