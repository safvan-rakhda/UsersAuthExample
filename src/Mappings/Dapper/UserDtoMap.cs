using Dapper.FluentMap.Mapping;
using UsersAuthExample.Data.Dto;

namespace UsersAuthExample.Mappings.Dapper
{
    public class UserDtoMap : EntityMap<UserDto>
    {
        public UserDtoMap()
        {
            Map(u => u.UserId).ToColumn("c_UserId", false);
            Map(u => u.UserName).ToColumn("c_UserName", false);
            Map(u => u.FirstName).ToColumn("c_FirstName", false);
            Map(u => u.LastName).ToColumn("c_LastName", false);
            Map(u => u.Role).ToColumn("c_Role", false);
            Map(u => u.CreatedDate).ToColumn("c_Createdat", false);
            Map(u => u.ModifiedDate).ToColumn("c_Modifiedat", false);
            Map(u => u.ModifiedBy).ToColumn("c_ModifiedBy", false);
        }
    }
}
