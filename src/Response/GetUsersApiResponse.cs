using System.Collections.Generic;
using UsersAuthExample.Services.ServiceResponses;

namespace UsersAuthExample.Response
{
    public class GetUsersApiResponse
    {
        public IEnumerable<User> Items { get; set; }
    }
}
