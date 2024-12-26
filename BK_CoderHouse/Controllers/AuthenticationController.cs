using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BK_CoderHouse.Application.Interfaces;
using BK_CoderHouse.Domain.Payload;
using BK_CoderHouse.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace BK_CoderHouse.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController (IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> loginOwner([FromBody] LoginPayload payload) => Ok(await _authenticationService.Login(payload));

        [HttpPost("reflesh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefleshTokenPayload payload) => Ok(await _authenticationService.refleshLogin(payload));

    }
}
