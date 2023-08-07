using Identity.Service.EventHandlers.Commands;
using Identity.Service.EventHandlers.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("v1/identity")]
    public class IdentityController : ControllerBase
    {
        #region Variables
        private readonly ILogger<IdentityController> _logger;
        private readonly IMediator _mediator;
        #endregion

        #region Constructor
        public IdentityController(
            ILogger<IdentityController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        #endregion

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(bool))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create(UserCreateCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("authentication")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IdentityAccess))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Authentication(UserLoginCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);

                if (!result.Succeeded)
                {
                    return BadRequest("Access denied");
                }

                return Ok(result);
            }

            return BadRequest();
        }
    }
}
