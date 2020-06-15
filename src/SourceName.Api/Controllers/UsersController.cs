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
        /// Registers a new User
        /// </summary>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}