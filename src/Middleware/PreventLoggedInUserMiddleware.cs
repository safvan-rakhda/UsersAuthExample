using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using UsersAuthExample.Attributes;
using UsersAuthExample.Extensions.RequestBodyValidators;

namespace UsersAuthExample.Middleware
{
    public class PreventLoggedInUserMiddleware
    {
        private readonly RequestDelegate _next;

        public PreventLoggedInUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var attribute = endpoint?.Metadata.GetMetadata<PreventLoggedInUser>();
            if (attribute == null)
            {
                await _next(context);
                return;
            }

            if (context.Request.IsLoggedInUser(attribute.FetchFromRoutes))
            {
                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                return;
            }

            await _next(context);
            return;
        }
    }
}
