using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MediatR;

using SourceName.Application.Users.Commands;

namespace SourceName.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UsersController : ApiControllerBase
    {
        public UsersController(IMediator mediator) : base(mediator) { }

        /// <summary>
        /// Authenticates an existing User and returns an auth token (JWT)
        /// </summary>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserCommand command)
        {
            var token = await _mediator.Send(command);
            return Ok(token);
        }

        /// <summary>
        /// Logs the current user out, expiring the auth token (JWT)
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await _mediator.Send(new LogOutCommand());
            return NoContent();
        }

        /// <summary>
        /// Registers a new User
        /// </summary>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("test-auth")]
        public Task<IActionResult> TestAuth()
        {
            return Task.FromResult<IActionResult>(Ok("Authorized"));
        }
    }
}