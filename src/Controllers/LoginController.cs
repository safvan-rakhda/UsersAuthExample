using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UsersAuthExample.Request;
using UsersAuthExample.Response;
using UsersAuthExample.Services.Interfaces;
using UsersAuthExample.Services.ServiceRequest;

namespace UsersAuthExample.Controllers
{
    [Route("api/user")]
    public class LoginController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public LoginController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [Route("signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInApiRequest request)
        {
            var serviceRequest = _mapper.Map<AuthenticateUserServiceRequest>(request);
            var serviceResponse = await _userService.AuthenticateUser(serviceRequest);

            if (serviceResponse.IsValid)
            {
                var apiResponse = _mapper.Map<UserSignInApiResponse>(serviceResponse);
                return Ok(apiResponse);
            }

            return StatusCode((int)serviceResponse.HttpStatusCode, serviceResponse.Errors);

        }
    }
}
