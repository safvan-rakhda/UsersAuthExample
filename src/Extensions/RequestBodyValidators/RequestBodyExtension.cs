using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using static UsersAuthExample.Data.Constants;

namespace UsersAuthExample.Extensions.RequestBodyValidators
{
    public static class RequestBodyExtension
    {
        public static bool IsLoggedInUser(this HttpRequest request, bool fetchFromRoutes)
        {
            var claim = request.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == CustomClaimTypes.UserId);

            if (claim == null || !int.TryParse(claim.Value, out int userId))
            {
                return false;
            }

            if (fetchFromRoutes)
                return Convert.ToInt64(request.RouteValues["userId"]) == userId;

            return false;
        }
    }
}
