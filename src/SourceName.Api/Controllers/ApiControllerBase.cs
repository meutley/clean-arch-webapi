using Microsoft.AspNetCore.Mvc;

using MediatR;

namespace SourceName.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly IMediator _mediator;

        public ApiControllerBase(IMediator mediator) => _mediator = mediator;
    }
}