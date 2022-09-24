using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UsersAuthExample.Attributes;
using UsersAuthExample.Request;
using UsersAuthExample.Response;
using UsersAuthExample.Services.Interfaces;
using UsersAuthExample.Services.ServiceRequest;

namespace UsersAuthExample.Controllers
{
    //[Route("api/v{version:apiVersion}/users")]
    [Route("api/users")]
    [ApiVersion("1.0")]
    [Authorize]
    public class UsersController : Controller
    {
        public readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getbyid/{userId}")]
        public async Task<IActionResult> GetById(int userId)
        {
            return Ok(await _userService.GetUserById(userId));
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserApiRequest request)
        {
            var serviceRequest = _mapper.Map<CreateUserServiceRequest>(request);
            var serviceResponse = await _userService.CreateUser(serviceRequest);

            return Ok(serviceResponse);
        }

        [HttpPost]
        [Route("get")]
        public async Task<IActionResult> Get()
        {
            var serviceResponse = await _userService.GetUsers();

            if (serviceResponse.IsValid)
            {
                var apiResponse = _mapper.Map<GetUsersApiResponse>(serviceResponse);
                return Ok(apiResponse);
            }

            return StatusCode((int)serviceResponse.HttpStatusCode, serviceResponse.Errors);
        }


        [PreventLoggedInUser(FetchFromRoutes = true)]
        [HttpPost]
        [Route("deletebyid/{userId}")]
        public async Task<IActionResult> DeleteById(int userId)
        {
            return Ok(await _userService.DeleteUsersAsync(new[] { userId }));
        }
    }
}
