using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UsersAuthExample.Request;
using UsersAuthExample.Services.Interfaces;
using UsersAuthExample.Services.ServiceRequest;

namespace UsersAuthExample.Controllers
{
    //[Route("api/v{version:apiVersion}/users")]
    [Route("api/users")]
    [ApiVersion("1.0")]
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
        public async Task<IActionResult> CreateUser([FromBody] CreateUserApiRequest request)
        {
            var serviceRequest = _mapper.Map<CreateUserServiceRequest>(request);
            var serviceResponse = await _userService.CreateUser(serviceRequest);

            return Ok(serviceResponse);
        }
    }
}
